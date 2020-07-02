using LiteDB;
using MonthlySubscriptions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MonthlySubscriptions.Services
{
    public static class Repository
    {

        private static readonly string dbPath = $"{FileSystem.AppDataDirectory}/main.db";
        
        public static void Save(MonthData data)
        {
            data.YearMonth = data.YearMonth.StripMonthYear();
            using var db = new LiteDatabase(dbPath);
            var collection = db.GetCollection<MonthData>();
            if (!collection.Update(data)) collection.Insert(data);
        }

        public static void Remove(MonthData data) => Remove(data.YearMonth);

        public static void Remove(DateTime month)
        {
            month = month.StripMonthYear();
            using var db = new LiteDatabase(dbPath);
            db.GetCollection<MonthData>().Delete(month);
        }

        public static MonthData Get(DateTime date)
        {
            date = date.StripMonthYear();
            using var db = new LiteDatabase(dbPath);
            var res = db.GetCollection<MonthData>().FindById(date);
            return res ?? new MonthData {  YearMonth = date };
        }

        public static void BackupTo(string location)
        {
            File.Copy(dbPath, location, true);
        }

        public static void Restore(string location)
        {
            File.Copy(location, dbPath, true);
        }

        public static void ClearDatabase() => File.Delete(dbPath);

        public static void VerifyCurrentMonthPopulated()
        {
            var date = DateTime.Now.StripMonthYear();
            using var db = new LiteDatabase(dbPath);
            var collection = db.GetCollection<MonthData>();
            var currentData = collection.FindById(date);

            if (currentData is null)
                currentData = new MonthData {
                    YearMonth = date
                };
            else if (currentData.IsBlastedFromThePast) return;

            MonthData sourceData;

            int retryCount = 0;
            do
            {
                date = date.AddMonths(-1);
                sourceData = db.GetCollection<MonthData>().FindById(date);
            } while (sourceData?.Subscriptions?.Any() != true && ++retryCount != 10);

            if (retryCount == 10)
            {
                return;
            }

            foreach (var sub in sourceData.Subscriptions)
            {
                var cancels = currentData.CanceledSubscriptions.GetValueOrDefault(sub.Key);
                var subsToAdd = sub.Value.Where(s => !cancels.Any(c => c.Title == s.Title));
                if (subsToAdd.Any())
                    currentData.Subscriptions[sub.Key] = subsToAdd;
            }

            currentData.IsBlastedFromThePast = true;
            collection.Update(currentData);
        }

    }
}
;