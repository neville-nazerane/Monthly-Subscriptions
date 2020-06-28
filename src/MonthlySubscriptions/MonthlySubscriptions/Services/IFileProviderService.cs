﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MonthlySubscriptions.Services
{
    public interface IFileProviderService
    {
        string GetBackupPath();
        string[] GetDirectories();
    }
}
