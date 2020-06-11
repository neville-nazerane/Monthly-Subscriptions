using LiteDB;
using MonthlySubscriptions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MonthlySubscriptions.Services
{
    public static class Repository
    {

        static readonly string dbPath = $"{FileSystem.AppDataDirectory}/main.db";
        
        public static void Save(MonthData data)
        {
            data.YearMonth = StripDate(data.YearMonth);
            using var db = new LiteDatabase(dbPath);
            var collection = db.GetCollection<MonthData>();
            if (!collection.Update(data)) collection.Insert(data);
        }

        public static void Remove(MonthData data) => Remove(data.YearMonth);

        public static void Remove(DateTime month)
        {
            month = StripDate(month);
            using var db = new LiteDatabase(dbPath);
            db.GetCollection<MonthData>().Delete(month);
        }

        public static MonthData Get(DateTime date)
        {
            date = StripDate(date);
            using var db = new LiteDatabase(dbPath);
            var res = db.GetCollection<MonthData>().FindById(date);
            return res ?? new MonthData {  YearMonth = date };
        }

        public static DateTime StripDate(DateTime date)
        {
            string format = "MM-YYYY";
            string strDate = date.ToString(format);
            return DateTime.ParseExact(strDate, format, null);
        }

    }
}
;