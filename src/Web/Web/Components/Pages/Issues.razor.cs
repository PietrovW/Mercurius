using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Web.Models;

namespace Web.Components.Pages;

public partial class Issues
{
    IQueryable<Country>? items;
    PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
    string nameFilter = string.Empty;

    //GridSort<Country> rankSort = GridSort<Country>
    //    .ByDescending(x => x.Medals.Gold)
    //    .ThenDescending(x => x.Medals.Silver)
    //    .ThenDescending(x => x.Medals.Bronze);

    
    GridSort<Country> nameSort = GridSort<Country>.ByAscending(x => x.Name, StringLengthComparer.Instance);


    IQueryable<Country>? FilteredItems => items?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));

    protected override async Task OnInitializedAsync()
    {
        //items = (await Data.GetCountriesAsync()).AsQueryable();
    }

    private void HandleCountryFilter(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            nameFilter = value;
        }
    }

    private void HandleClear()
    {
        if (string.IsNullOrWhiteSpace(nameFilter))
        {
            nameFilter = string.Empty;
        }
    }

    public class StringLengthComparer : IComparer<string>
    {
        public static readonly StringLengthComparer Instance = new StringLengthComparer();

        public int Compare(string? x, string? y)
        {
            if (x is null)
            {
                return y is null ? 0 : -1;
            }

            if (y is null)
            {
                return 1;
            }

            return x.Length.CompareTo(y.Length);
        }
    }

    private void HandleRowFocus(FluentDataGridRow<Country> row)
    {
        Console.WriteLine($"Row focused: {row.Item?.Name}");
    }
}
