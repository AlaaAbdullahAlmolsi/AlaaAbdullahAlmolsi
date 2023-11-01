using System;
using System.Collections.Generic;

namespace OrderSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            Cart cart = new Cart();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Fast Food Ordering System");
                Console.WriteLine("1. View Menu");
                Console.WriteLine("2. Add Item to Cart");
                Console.WriteLine("3. Remove Item from Cart");
                Console.WriteLine("4. View Cart");
                Console.WriteLine("5. Checkout");
                Console.WriteLine("6. Exit");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                int choice;
                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        menu.DisplayMenu();
                        break;
                    case 2:
                        Console.Write("Enter the item ID to add to the cart: ");
                        input = Console.ReadLine();

                        int itemId;
                        if (!int.TryParse(input, out itemId))
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            continue;
                        }

                        var item = menu.GetItem(itemId);
                        if (item == null)
                        {
                            Console.WriteLine("Item not found in the menu.");
                        }
                        else
                        {
                            Console.Write("Enter the quantity: ");
                            input = Console.ReadLine();

                            int quantity;
                            if (!int.TryParse(input, out quantity) || quantity <= 0)
                            {
                                Console.WriteLine("Invalid input. Please enter a positive number.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                continue;
                            }

                            cart.AddItem(item, quantity);
                            Console.WriteLine(item.Name + " added to cart.");
                        }
                        break;
                    case 3:
                        Console.Write("Enter the item ID to remove from the cart: ");
                        input = Console.ReadLine();

                        if (!int.TryParse(input, out itemId))
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            continue;
                        }

                        cart.RemoveItem(itemId);
                        break;
                    case 4:
                        cart.DisplayCart();
                        break;
                    case 5:
                        cart.Checkout();
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    abstract class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public MenuItem(int id, string name, double price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public abstract void Display();
    }

    class Item : MenuItem
    {
        public Item(int id, string name, double price)
            : base(id, name, price)
        {
        }

        public override void Display()
        {
            Console.WriteLine("ID: " + Id + ", " + Name + " ($" + Price + ")");
        }
    }

    class Menu
    {
        private List<MenuItem> items;

        public Menu()
        {
            items = new List<MenuItem>();
            // Add some sample items to the menu
            items.Add(new Item(1, "Burger", 5.99));
            items.Add(new Item(2, "French Fries", 2.99));
            items.Add(new Item(3, "Soft Drink", 1.99));
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Menu:");
            foreach (MenuItem item in items)
            {
                item.Display();
            }
        }

        public MenuItem GetItem(int itemId)
        {
            return items.Find(item => item.Id == itemId);
        }
    }

    abstract class CartItem
    {
        public MenuItem MenuItem { get; set; }

        public CartItem(MenuItem menuItem)
        {
            MenuItem = menuItem;
        }

        public abstract void Display();
    }

    class CartItemWithQuantity : CartItem
    {
        public int Quantity { get; set; }

        public CartItemWithQuantity(MenuItem menuItem, int quantity)
            : base(menuItem)
        {
            Quantity = quantity;
        }

        public override void Display()
        {
            Console.WriteLine("ID: " + MenuItem.Id + ", " + MenuItem.Name + " ($" + MenuItem.Price + ") x " + Quantity);
        }
    }

    class Cart
    {
        private List<CartItemWithQuantity> items;

        public Cart()
        {
            items = new List<CartItemWithQuantity>();
        }

        public void AddItem(MenuItem item, int quantity)
        {
            var existingItem = items.Find(cartItem => cartItem.MenuItem.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItemWithQuantity(item, quantity));
            }
        }

        public void RemoveItem(int itemId)
        {
            var itemToRemove = items.Find(cartItem => cartItem.MenuItem.Id == itemId);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                Console.WriteLine(itemToRemove.MenuItem.Name + " removed from cart.");
            }
            else
            {
                Console.WriteLine("Item not found in the cart.");
            }
        }

        public void DisplayCart()
        {
            Console.WriteLine("Cart:");
            foreach (CartItemWithQuantity item in items)
            {
                item.Display();
            }
        }

        public void Checkout()
        {
            double total = 0;
            foreach (CartItemWithQuantity item in items)
            {
                total += item.MenuItem.Price * item.Quantity;
            }
            Console.WriteLine("Total: $" + total);
            Console.WriteLine("Thank you for using Fast Food Ordering System!");
            items.Clear();
        }
    }
}