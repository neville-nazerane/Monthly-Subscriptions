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


    }
}
;