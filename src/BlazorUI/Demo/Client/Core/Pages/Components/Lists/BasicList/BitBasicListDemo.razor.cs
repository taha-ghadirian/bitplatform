﻿namespace Bit.BlazorUI.Demo.Client.Core.Pages.Components.Lists.BasicList;

public partial class BitBasicListDemo
{
    private readonly List<ComponentParameter> componentParameters = new()
    {
        new()
        {
            Name = "EnableVirtualization",
            Type = "bool",
            DefaultValue = "false",
            Description = "Enables virtualization in rendering the list.",
        },
        new()
        {
            Name = "Items",
            Type = "ICollection<TItem>",
            DefaultValue = "new Array.Empty<TItem>()",
            Description = "Gets or sets the list of items to render.",
        },
        new()
        {
            Name = "ItemSize",
            Type = "float",
            DefaultValue = "50",
            Description = "Gets the size of each item in pixels. Defaults to 50px.",
        },
        new()
        {
            Name = "OverscanCount",
            Type = "int",
            DefaultValue = "3",
            Description = "Gets or sets a value that determines how many additional items will be rendered before and after the visible region.",
        },
        new()
        {
            Name = "Role",
            Type = "string",
            DefaultValue = "list",
            Description = "Gets or set the role attribute of the BasicList html element.",
        },
        new()
        {
            Name = "RowTemplate",
            Type = "RenderFragment<TItem>?",
            DefaultValue = "null",
            Description = "Gets or sets the Template to render each row.",
        },
        new()
        {
            Name = "ItemsProvider",
            Type = "BitBasicListItemsProvider<TItem>?",
            DefaultValue = "null",
            Description = @"A callback that supplies data for the rid.
                            You should supply either Items or ItemsProvider, but not both.",
        },
        new()
        {
            Name = "VirtualizePlaceholder",
            Type = "RenderFragment<PlaceholderContext>?",
            DefaultValue = "null",
            Description = "Optional custom template for placeholder Text.",
        },
    };



    private List<Person> LotsOfPeople = Enumerable.Range(0, 8000).Select(i => new Person
    {
        Id = i + 1,
        FirstName = $"Person {i + 1}",
        LastName = $"Person Family {i + 1}",
        Job = $"Programmer {i + 1}"
    }).ToList();

    private List<Person> FewPeople = Enumerable.Range(0, 100).Select(i => new Person
    {
        Id = i + 1,
        FirstName = $"Person {i + 1}",
        LastName = $"Person Family {i + 1}",
        Job = $"Programmer {i + 1}"
    }).ToList();

    private BitBasicListItemsProvider<ProductDto> ProductsProvider = default!;
    private BitBasicListItemsProvider<CategoryOrProductDto> CategoriesAndProductsProvider = default!;

    protected override void OnInitialized()
    {
        ProductsProvider = async req =>
        {
            try
            {
                var query = new Dictionary<string, object>()
                                {
                    { "$top", req.Count},
                    { "$skip", req.StartIndex }
                                };

                var url = NavManager.GetUriWithQueryParameters("Products/GetProducts", query);

                var data = await HttpClient.GetFromJsonAsync(url, AppJsonContext.Default.PagedResultProductDto);

                return BitBasicListItemsProviderResult.From(data!.Items, (int)data!.TotalCount);
            }
            catch
            {
                return BitBasicListItemsProviderResult.From<ProductDto>(new List<ProductDto> { }, 0);
            }
        };

        CategoriesAndProductsProvider = async req =>
        {
            try
            {
                var query = new Dictionary<string, object>()
                            {
                    { "$top", req.Count},
                    { "$skip", req.StartIndex }
                            };

                var url = NavManager.GetUriWithQueryParameters("Products/GetCategoriesAndProducts", query);

                var data = await HttpClient.GetFromJsonAsync(url, AppJsonContext.Default.PagedResultCategoryOrProductDto);

                return BitBasicListItemsProviderResult.From(data!.Items, (int)data!.TotalCount);
            }
            catch
            {
                return BitBasicListItemsProviderResult.From<CategoryOrProductDto>(new List<CategoryOrProductDto> { }, 0);
            }
        };

        base.OnInitialized();
    }



    private readonly string example1RazorCode = @"
<BitBasicList Items=""LotsOfPeople""
              EnableVirtualization=""true""
              Style=""border: 1px #a19f9d solid; border-radius: 3px;"">
    <RowTemplate Context=""person"">
        <div @key=""person.Id"" style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px; margin: 10px;"">
            <img src=""https://picsum.photos/100/100?random=@(person.Id)"">
            <div style=""margin-left:3%; display: inline-block;"">
                <p>Id: <strong>@person.Id</strong></p>
                <p>Full Name: <strong>@person.FirstName @person.LastName</strong></p>
                <p>Job: <strong>@person.Job</strong></p>
            </div>
        </div>
    </RowTemplate>
</BitBasicList>";
    private readonly string example1CsharpCode = @"
private List<Person> LotsOfPeople = Enumerable.Range(0, 8000).Select(i => new Person
{
    Id = i + 1,
    FirstName = $""Person {i + 1}"",
    LastName = $""Person Family {i + 1}"",
    Job = $""Programmer {i + 1}""
}).ToList();


public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Job { get; set; }
}";

    private readonly string example2RazorCode = @"
<BitBasicList Items=""FewPeople"" Style=""border: 1px #a19f9d solid; border-radius: 3px;"">
    <RowTemplate Context=""person"">
        <div style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px; margin: 10px;"">
            <img src=""https://picsum.photos/100/100?random=@(person.Id)"">
            <p>Id: <strong>@person.Id</strong></p>
            <p>Full Name: <strong>@person.FirstName @person.LastName</strong></p>
            <p>Job: <strong>@person.Job</strong></p>
        </div>
    </RowTemplate>
</BitBasicList>";
    private readonly string example2CsharpCode = @"
private List<Person> FewPeople = Enumerable.Range(0, 100).Select(i => new Person
{
    Id = i + 1,
    FirstName = $""Person {i + 1}"",
    LastName = $""Person Family {i + 1}"",
    Job = $""Programmer {i + 1}""
}).ToList();


public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Job { get; set; }
}";

    private readonly string example3RazorCode = @"
<style>
    .list-item {
        padding: 16px 20px;
        background-color: #f2f2f2;
        margin: 10px 10px;
        width: 20%;
        height: 143px;
        display: inline-grid;
        justify-content: center;
        align-items: center;
    }
</style>

<BitBasicList Items=""LotsOfPeople"" Role=""list""
              EnableVirtualization=""true""
              Style=""border: 1px #a19f9d solid; border-radius: 3px;"">
    <RowTemplate Context=""person"">
        <div @key=""person.Id"" class=""list-item"">
            <span>Id: <strong>@person.Id</strong></span>
            <span>Full Name: <strong>@person.FirstName</strong></span>
            <span>Job: <strong>@person.Job</strong></span>
        </div>
    </RowTemplate>
</BitBasicList>";
    private readonly string example3CsharpCode = @"
private List<Person> LotsOfPeople = Enumerable.Range(0, 8000).Select(i => new Person
{
    Id = i + 1,
    FirstName = $""Person {i + 1}"",
    LastName = $""Person Family {i + 1}"",
    Job = $""Programmer {i + 1}""
}).ToList();


public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Job { get; set; }
}";

    private readonly string example4RazorCode = @"
<BitBasicList Items=""LotsOfPeople"" ItemSize=""300"" OverscanCount=""5""
              EnableVirtualization=""true""
              Style=""border: 1px #a19f9d solid; border-radius: 3px;"">
    <RowTemplate Context=""person"">
        <div @key=""person.Id"" style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px; margin: 10px;"">
            <p>Id: <strong>@person.Id</strong></p>
            <p>Full Name: <strong>@person.FirstName @person.LastName</strong></p>
            <p>Job: <strong>@person.Job</strong></p>
        </div>
    </RowTemplate>
</BitBasicList>";
    private readonly string example4CsharpCode = @"
private List<Person> LotsOfPeople = Enumerable.Range(0, 8000).Select(i => new Person
{
    Id = i + 1,
    FirstName = $""Person {i + 1}"",
    LastName = $""Person Family {i + 1}"",
    Job = $""Programmer {i + 1}""
}).ToList();


public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Job { get; set; }
}";

    private readonly string example5RazorCode = @"
<BitBasicList TItem=""ProductDto"" ItemSize=""83""
              EnableVirtualization=""true""
              ItemsProvider=""@ProductsProvider""
              Style=""border: 1px #a19f9d solid; border-radius: 3px;"">
    <RowTemplate Context=""product"">
        <div @key=""product.Id"" style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px;"">
            <div>Id: <strong>@product.Id</strong></div>
            <div>Name: <strong>@product.Name</strong></div>
            <div>Price: <strong>@product.Price</strong></div>
        </div>
    </RowTemplate>
    <VirtualizePlaceholder>
        <div style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px;"">
            <div>Id: <strong>Loading...</strong></div>
            <div>Name: <strong>Loading...</strong></div>
            <div>Price: <strong>Loading...</strong></div>
        </div>
    </VirtualizePlaceholder>
</BitBasicList>";
    private readonly string example5CsharpCode = @"
private BitBasicListItemsProvider<ProductDto> ProductsProvider;

protected override void OnInitialized()
{
    ProductsProvider = async req =>
    {
        try
        {
            var query = new Dictionary<string, object>()
                     {
                 { ""$top"", req.Count},
                 { ""$skip"", req.StartIndex }
                     };
    
            var url = NavManager.GetUriWithQueryParameters(""Products/GetProducts"", query);
    
            var data = await HttpClient.GetFromJsonAsync(url, AppJsonContext.Default.PagedResultProductDto);
    
            return BitBasicListItemsProviderResult.From(data!.Items, (int)data!.TotalCount);
        }
        catch
        {
            return BitBasicListItemsProviderResult.From<ProductDto>(new List<ProductDto> { }, 0);
        }
    };

    base.OnInitialized();
}


public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}

[JsonSerializable(typeof(PagedResult<ProductDto>))]
public partial class AppJsonContext : JsonSerializerContext { }";

    private readonly string example6RazorCode = @"
<BitBasicList TItem=""CategoryOrProductDto"" ItemSize=""83""
              EnableVirtualization=""true""
              ItemsProvider=""@CategoriesAndProductsProvider""
              Style=""border: 1px #a19f9d solid; border-radius: 3px;"">
    <RowTemplate Context=""catOrProd"">
        @if (catOrProd.IsProduct)
        {
            <div @key=""@($""{catOrProd.CategoryId}-{catOrProd.ProductId}"")"" style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px; display:flex; flex-flow:row;"">
                <div style=""width:250px;"">Name: <strong>@catOrProd.Name</strong></div>
                <div>Price: <strong>@catOrProd.Price</strong></div>
            </div>
        }
        else
        {
            <div @key=""catOrProd.CategoryId"" style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px;background-color:#777"">
                <div>@catOrProd.Name</div>
            </div>
        }
    </RowTemplate>
    <VirtualizePlaceholder>
        <div style=""border-bottom: 1px #8a8886 solid; padding: 5px 20px;"">
            Loading...
        </div>
    </VirtualizePlaceholder>
</BitBasicList>";
    private readonly string example6CsharpCode = @"
private BitBasicListItemsProvider<CategoryOrProductDto> CategoriesAndProductsProvider;

protected override void OnInitialized()
{
    CategoriesAndProductsProvider = async req =>
    {
        try
        {
            var query = new Dictionary<string, object>()
                {
                { ""$top"", req.Count},
                { ""$skip"", req.StartIndex }
                };

            var url = NavManager.GetUriWithQueryParameters(""Products/GetCategoriesAndProducts"", query);

            var data = await HttpClient.GetFromJsonAsync(url, AppJsonContext.Default.PagedResultCategoryOrProductDto);

            return BitBasicListItemsProviderResult.From(data!.Items, (int)data!.TotalCount);
        }
        catch
        {
            return BitBasicListItemsProviderResult.From<CategoryOrProductDto>(new List<CategoryOrProductDto> { }, 0);
        }
    };

    base.OnInitialized();
}


public class CategoryOrProductDto
{
    public int? ProductId { get; set; }
    public int? CategoryId { get; set; }
    public bool IsProduct => ProductId is not null;
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}

[JsonSerializable(typeof(PagedResult<CategoryOrProductDto>))]
public partial class AppJsonContext : JsonSerializerContext { }";
}
