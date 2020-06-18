using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MonthlySubscriptions.Utils
{
    public static class ViewModelExtensions
    {

        public static TViewModel GetViewModel<TViewModel>(this IViewModelUtil<TViewModel> util)
        {
            if (util is Page page)
            {
                return (TViewModel)page.BindingContext;
            }
            else throw new Exception("This function only supports pages");
        }

    }
}
