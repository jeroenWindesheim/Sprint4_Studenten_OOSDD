
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }

        private List<GroceryListItem> GetGroceryListItemsByProductId(int productId)
        {
            return _groceryListItemsRepository
                .GetAll()
                .Where(groceryListItem => groceryListItem.ProductId == productId)
                .ToList();
        }
        
        private BoughtProducts? CreateBoughtProduct(GroceryListItem groceryListItem, Product product)
        {
            GroceryList? groceryList = _groceryListRepository.Get(groceryListItem.GroceryListId);
            if (groceryList == null) return null;

            Client? client = _clientRepository.Get(groceryList.ClientId);
            if (client == null) return null;

            return new BoughtProducts(client, groceryList, product);
        }

        public List<BoughtProducts> Get(int productId)
        {
            Product? product = _productRepository.Get(productId);
            if (product == null) return [];
            
            List<GroceryListItem> groceryListItems = GetGroceryListItemsByProductId(productId);
            
            List<BoughtProducts> boughtProducts = groceryListItems
                .Select(groceryListItem => CreateBoughtProduct(groceryListItem, product))
                .OfType<BoughtProducts>()
                .ToList();
            
            return boughtProducts;
        }
        
        
        
        // public List<BoughtProducts> Get2(int productId)
        // {
        //     Product? product = _productRepository.Get(productId);
        //     if (product == null) return [_boughtProductsList];
        //     
        //     List<GroceryListItem> groceryListItemRepository = _groceryListItemsRepository.GetAll().Where(glir => glir.ProductId == productId).ToList();
        //     foreach (GroceryListItem glirItem in groceryListItemRepository)
        //     {
        //         // Get GroceryList
        //         GroceryList groceryList = _groceryListRepository.Get(glirItem.GroceryListId);
        //         if (groceryList == null) break;
        //         // Get client
        //         Client client = _clientRepository.Get(groceryList.ClientId);
        //         if (client == null) break;
        //         _boughtProductsList.Add(new BoughtProducts(client, groceryList, product));
        //     }
        //     return _boughtProductsList;
        // }
    }
}
