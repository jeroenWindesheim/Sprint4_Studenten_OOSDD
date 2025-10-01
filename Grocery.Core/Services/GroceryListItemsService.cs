using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            // Make a list to return
            List<BestSellingProducts> bestSold = [];
            
            // Get all product
            List<Product> allProducts = _productRepository.GetAll();
            List<GroceryListItem> groceries = _groceriesRepository.GetAll();
            // List<GroceryListItem> groceries = _groceriesRepository.GetAll();
            
            int count = 1;
            foreach (Product item in allProducts)
            {
                int amountOfProductsSold = 0;
                foreach (GroceryListItem groceryListItem in groceries)
                {
                    
                    if (item.Id == groceryListItem.ProductId)
                    {
                        // var test = _groceriesRepository.GetAllOnGroceryListId(groceryListItem.ProductId);
                        amountOfProductsSold += groceryListItem.Amount;
                    }
                }
                BestSellingProducts newProduct = new BestSellingProducts(item.Id, item.Name, item.Stock, amountOfProductsSold, 0);
                count++;
                bestSold.Add(newProduct);
            }
            // Sort by amount sold (descending), then take topX
            bestSold = bestSold
                .OrderByDescending<BestSellingProducts, object>(p => p.NrOfSells) // adjust if property name differs
                .Take(topX)
                .ToList();

            // Re-assign ranks after ordering
            int rank = 1;
            foreach (var product in bestSold)
            {
                product.Ranking = rank++;
            }
            

            return bestSold;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
