using FinalProjectBrianGallenberger.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectBrianGallenberger
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            var db = new NorthwindContext();
            string choice;
            do
            {
                //show all options
                Console.WriteLine("1) Add Product");
                Console.WriteLine("2) Edit Product");
                Console.WriteLine("3) Display All Products");
                Console.WriteLine("4) Display Specific Product");
                Console.WriteLine("5) Add Category");
                Console.WriteLine("6) Edit Category");
                Console.WriteLine("7) Display All Categorys");
                Console.WriteLine("8) Display All Categorys and their Active Products");
                Console.WriteLine("9) Display Specific Category");
                Console.WriteLine("\"q\" to quit");
                choice = Console.ReadLine();

                logger.Info($"Option {choice} selected");
                var categorys = db.Categories.OrderBy(c => c.CategoryId);
                var products = db.Products.OrderBy(p => p.ProductID);
                var suppliers = db.Suppliers.OrderBy(s => s.SupplierId);

                int count = 0;
                int number = 0;
                var isValid = false;
                var stringOrNumber = false;
                switch (choice)
                {
                    //add a product
                    case "1":
                        Product product = new Product();

                        //check if that product already exists
                        isValid = false;
                        String productNameValidation = "";
                        while (isValid == false)
                        {
                            isValid = true;
                            Console.WriteLine("Enter Product Name:");
                            productNameValidation = Console.ReadLine();
                            foreach (var prod in products)
                            {
                                if (productNameValidation == prod.ProductName)
                                {
                                    isValid = false;
                                    Console.WriteLine("Product already exists");
                                }
                            }
                        }

                        product.ProductName = productNameValidation;
                        Console.WriteLine("Enter Quantity Per Unit:");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter Unit Price:");
                        product.UnitPrice = Decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Units In Stock:");
                        product.UnitsInStock = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Units On Order:");
                        product.UnitsOnOrder = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Reorder Level:");
                        product.ReorderLevel = Int16.Parse(Console.ReadLine());

                        String categoryName = "";
                        count = 0;
                        foreach (var cat in categorys)
                        {
                            count++;
                        }
                        //check that category exists
                        var realCategory = false;
                        while (!realCategory)
                        {
                            count = 0;
                            foreach (var cat in categorys)
                            {
                                Console.WriteLine(++count + ")" + cat.CategoryName);
                            }
                            Console.WriteLine("Enter a Category");
                            categoryName = Console.ReadLine();

                            //check if string or number
                            number = 0;
                            count = 0;
                            stringOrNumber = Int32.TryParse(categoryName, out number);
                            foreach (var cat in categorys)
                            {
                                count++;
                                if (number == count || categoryName.ToLower() == cat.CategoryName.ToLower())
                                {
                                    product.CategoryId = cat.CategoryId;
                                    realCategory = true;
                                }
                            }
                        }
                        product.Discontinued = false;

                        String supplierName = "";
                        count = 0;
                        foreach (var sup in suppliers)
                        {
                            count++;
                        }
                        //check that supplier exists
                        var realSupplier = false;
                        while (!realSupplier)
                        {
                            count = 0;
                            foreach (var sup in suppliers)
                            {
                                Console.WriteLine(++count + ") " + sup.CompanyName);
                            }
                            Console.WriteLine("Enter a Supplier name or number");
                            supplierName = Console.ReadLine();

                            //check if string or number
                            number = 0;
                            count = 0;
                            stringOrNumber = Int32.TryParse(supplierName, out number);
                            foreach (var sup in suppliers)
                            {
                                count++;
                                if (number == count || supplierName.ToLower() == sup.CompanyName.ToLower())
                                {
                                    product.SupplierId = sup.SupplierId;
                                    realSupplier = true;
                                }
                            }
                        }

                        db.AddProduct(product);
                        logger.Info("Product added - {name}", productNameValidation);
                        break;

                    //edit product
                    case "2":
                        // display all products
                        count = 0;
                        foreach (var prod in products)
                        {
                            count++;
                            Console.WriteLine(count + ")" + prod.ProductName);
                        }
                        if (count == 0)
                        {
                            Console.WriteLine("No Products");
                        }
                        else
                        {
                            var valid = false;
                            Product productToEdit = new Product();
                            do
                            {
                                Console.WriteLine(count + " Products returned");
                                Console.WriteLine("Enter name or number");
                                choice = Console.ReadLine();

                                //check if string or number
                                number = 0;
                                stringOrNumber = Int32.TryParse(choice, out number);
                                count = 0;
                                foreach (var prod in products)
                                {
                                    count++;
                                    if (count == number || choice.ToLower() == prod.ProductName.ToLower())
                                    {
                                        valid = true;
                                        productToEdit.ProductID = prod.ProductID;
                                        productToEdit.ProductName = prod.ProductName;
                                        productToEdit.QuantityPerUnit = prod.QuantityPerUnit;
                                        productToEdit.UnitPrice = prod.UnitPrice;
                                        productToEdit.UnitsInStock = prod.UnitsInStock;
                                        productToEdit.UnitsOnOrder = prod.UnitsOnOrder;
                                        productToEdit.ReorderLevel = prod.ReorderLevel;
                                        productToEdit.Discontinued = prod.Discontinued;
                                        productToEdit.CategoryId = prod.CategoryId;
                                        productToEdit.SupplierId = prod.SupplierId;
                                    }
                                }

                                if (!valid)
                                {
                                    Console.WriteLine("Enter a valid product name or number");
                                }
                            } while (!valid);
                            choice = "";
                            while (!(choice.ToLower() == "q"))
                            {
                                Console.WriteLine("Editing " + productToEdit.ProductName + "\n");
                                Console.WriteLine("1) Edit Product Name");
                                Console.WriteLine("2) Edit Product QuantityPerUnit");
                                Console.WriteLine("3) Edit Product UnitPrice");
                                Console.WriteLine("4) Edit Product UnitsInStock");
                                Console.WriteLine("5) Edit Product UnitsOnOrder");
                                Console.WriteLine("6) Edit Product ReorderLevel");
                                Console.WriteLine("7) Edit if Product is Discontinued");
                                Console.WriteLine("8) Edit Product Category Id");
                                Console.WriteLine("9) Edit Product Supplier Id");
                                Console.WriteLine("10) Edit All");
                                Console.WriteLine("11) Delete Product");
                                Console.WriteLine("\"q\" to quit");
                                choice = Console.ReadLine();
                                logger.Info($"Option {choice} selected");

                                switch (choice)
                                {
                                    case "1":
                                        //check if that product already exists
                                        isValid = false;
                                        String newProductName = "";
                                        while (isValid == false)
                                        {
                                            isValid = true;
                                            Console.WriteLine("Enter Product Name: ");
                                            newProductName = Console.ReadLine();
                                            foreach (var prod in products)
                                            {
                                                if (newProductName == prod.ProductName)
                                                {
                                                    isValid = false;
                                                    Console.WriteLine("Product already exists");
                                                }
                                            }
                                        }
                                        productToEdit.ProductName = newProductName;
                                        db.EditProduct(productToEdit);
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        break;
                                    case "2":
                                        //Edit Product QuantityPerUnit
                                        Console.WriteLine("Enter Quantity Per Unit:");
                                        productToEdit.QuantityPerUnit = Console.ReadLine();
                                        db.EditProduct(productToEdit);
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        break;
                                    case "3":
                                        //Edit Product UnitPrice
                                        Console.WriteLine("Enter Unit Price:");
                                        productToEdit.UnitPrice = Decimal.Parse(Console.ReadLine());
                                        db.EditProduct(productToEdit);
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        break;
                                    case "4":
                                        //Edit Product UnitsInStock
                                        Console.WriteLine("Enter Units In Stock:");
                                        productToEdit.UnitsInStock = Int16.Parse(Console.ReadLine());
                                        db.EditProduct(productToEdit);
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        break;
                                    case "5":
                                        //Edit Product UnitsOnOrder
                                        Console.WriteLine("Enter Units On Order:");
                                        productToEdit.UnitsOnOrder = Int16.Parse(Console.ReadLine());
                                        db.EditProduct(productToEdit);
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        break;
                                    case "6":
                                        //Edit Product ReorderLevel
                                        Console.WriteLine("Enter Reorder Level:");
                                        productToEdit.ReorderLevel = Int16.Parse(Console.ReadLine());
                                        db.EditProduct(productToEdit);
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        break;
                                    case "7":
                                        //Edit if Product is Discontinued
                                        Console.WriteLine("Product is Discontinued ('true' or 'false'):");
                                        productToEdit.Discontinued = Boolean.Parse(Console.ReadLine());
                                        db.EditProduct(productToEdit);
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        break;
                                    case "8":
                                        //check that category exists
                                        var validCategory = false;
                                        while (!validCategory)
                                        {
                                            count = 0;
                                            foreach (var cat in categorys)
                                            {
                                                Console.WriteLine(++count + ")" + cat.CategoryName);
                                            }
                                            Console.WriteLine("Enter a New Category");
                                            categoryName = Console.ReadLine();

                                            //check if string or number
                                            number = 0;
                                            stringOrNumber = Int32.TryParse(categoryName, out number);
                                            foreach (var cat in categorys)
                                            {
                                                if (number == cat.CategoryId || categoryName.ToLower() == cat.CategoryName.ToLower())
                                                {
                                                    productToEdit.CategoryId = cat.CategoryId;
                                                    realCategory = true;
                                                }
                                            }

                                        }
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        db.EditProduct(productToEdit);
                                        break;
                                    case "9":
                                        //edit supplier id
                                        //check that supplier exists
                                        realSupplier = false;
                                        while (!realSupplier)
                                        {
                                            count = 0;
                                            foreach (var sup in suppliers)
                                            {
                                                Console.WriteLine(++count + ") " + sup.CompanyName);
                                            }
                                            Console.WriteLine("Enter a Supplier name or number");
                                            supplierName = Console.ReadLine();

                                            //check if string or number
                                            number = 0;
                                            count = 0;
                                            stringOrNumber = Int32.TryParse(supplierName, out number);
                                            foreach (var sup in suppliers)
                                            {
                                                count++;
                                                if (number == count || supplierName.ToLower() == sup.CompanyName.ToLower())
                                                {
                                                    productToEdit.SupplierId = sup.SupplierId;
                                                    realSupplier = true;
                                                }
                                            }
                                        }
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        db.EditProduct(productToEdit);
                                        break;
                                    case "10":
                                        //edit all
                                        //check if that product already exists
                                        isValid = false;
                                        newProductName = "";
                                        while (isValid == false)
                                        {
                                            isValid = true;
                                            Console.WriteLine("Enter Product Name: ");
                                            newProductName = Console.ReadLine();
                                            foreach (var prod in products)
                                            {
                                                if (newProductName == prod.ProductName)
                                                {
                                                    isValid = false;
                                                    Console.WriteLine("Product already exists");
                                                }
                                            }
                                        }
                                        productToEdit.ProductName = newProductName;
                                        Console.WriteLine("Enter Quantity Per Unit:");
                                        productToEdit.QuantityPerUnit = Console.ReadLine();
                                        Console.WriteLine("Enter Unit Price:");
                                        productToEdit.UnitPrice = Decimal.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Units In Stock:");
                                        productToEdit.UnitsInStock = Int16.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Units On Order:");
                                        productToEdit.UnitsOnOrder = Int16.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Reorder Level:");
                                        productToEdit.ReorderLevel = Int16.Parse(Console.ReadLine());
                                        Console.WriteLine("Product is Discontinued ('true' or 'false'):");
                                        productToEdit.Discontinued = Boolean.Parse(Console.ReadLine());
                                        //check that category exists
                                        validCategory = false;
                                        while (!validCategory)
                                        {
                                            count = 0;
                                            foreach (var cat in categorys)
                                            {
                                                Console.WriteLine(++count + ")" + cat.CategoryName);
                                            }
                                            Console.WriteLine("Enter a New Category");
                                            categoryName = Console.ReadLine();

                                            //check if string or number
                                            number = 0;
                                            stringOrNumber = Int32.TryParse(categoryName, out number);
                                            if (number > 0)
                                            {
                                                foreach (var cat in categorys)
                                                {
                                                    if (number == cat.CategoryId)
                                                    {
                                                        productToEdit.CategoryId = cat.CategoryId;
                                                        realCategory = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (var cat in categorys)
                                                {
                                                    if (categoryName.ToLower() == cat.CategoryName.ToLower())
                                                    {
                                                        productToEdit.CategoryId = cat.CategoryId;
                                                        realCategory = true;
                                                    }
                                                }
                                            }
                                        }
                                        //check that supplier exists
                                        realSupplier = false;
                                        while (!realSupplier)
                                        {
                                            count = 0;
                                            foreach (var sup in suppliers)
                                            {
                                                Console.WriteLine(++count + ") " + sup.CompanyName);
                                            }
                                            Console.WriteLine("Enter a Supplier name or number");
                                            supplierName = Console.ReadLine();

                                            //check if string or number
                                            number = 0;
                                            count = 0;
                                            stringOrNumber = Int32.TryParse(supplierName, out number);
                                            foreach (var sup in suppliers)
                                            {
                                                count++;
                                                if (number == count || supplierName.ToLower() == sup.CompanyName.ToLower())
                                                {
                                                    productToEdit.SupplierId = sup.SupplierId;
                                                    realSupplier = true;
                                                }
                                            }
                                        }
                                        logger.Info("Product edited - {name}", productToEdit.ProductName);
                                        db.EditProduct(productToEdit);
                                        break;
                                    case "11":
                                        db.DeleteProduct(productToEdit);
                                        logger.Info("Product deleted - {name}", productToEdit.ProductName);
                                        choice = "q";
                                        break;
                                }
                            }
                        }
                        break;
                    case "3":
                        while (!(choice.ToLower() == "q"))
                        {
                            //user decides if they want to see all, active, or discontinued products
                            Console.WriteLine("1) Display All Products");
                            Console.WriteLine("2) Display Active Products");
                            Console.WriteLine("3) Display Discontinued Products");
                            Console.WriteLine("\"q\" to quit");
                            choice = Console.ReadLine();
                            logger.Info($"Option {choice} selected");

                            var query = db.Products.Where(p => p.ProductID > 0);
                            switch (choice)
                            {
                                case "1":
                                    // Display All Products
                                    query = db.Products.Where(p => p.ProductID > 0);
                                    break;
                                case "2":
                                    // Display Active Products
                                    query = db.Products.Where(p => p.Discontinued == false);
                                    break;
                                case "3":
                                    // Display Discontinued Products
                                    query = db.Products.Where(p => p.Discontinued == true);
                                    break;
                            }

                            count = 0;
                            foreach (var prod in query)
                            {
                                count++;
                                Console.Write(count + ")" + prod.ProductName + " - ");
                                if (prod.Discontinued)
                                {
                                    Console.WriteLine("Discontinued");
                                }
                                else
                                {
                                    Console.WriteLine("Active");
                                }

                            }
                            if (count == 0)
                            {
                                Console.WriteLine("No Products");
                            }
                            else
                            {
                                Console.WriteLine(count + " Products returned\n");
                            }
                        }
                        choice = " ";
                        break;
                    case "4":
                        //Display Specific Product
                        count = 0;
                        foreach (var prod in products)
                        {
                            count++;
                            Console.WriteLine(count + ") " + prod.ProductName);
                        }
                        if (count == 0)
                        {
                            Console.WriteLine("No Products");
                        }
                        else
                        {
                            Console.WriteLine(count + " Products returned");
                            var valid = false;
                            Product productToView = new Product();
                            do
                            {
                                Console.WriteLine("Enter name or number to view product");
                                choice = Console.ReadLine();

                                //check if string or number
                                number = 0;
                                stringOrNumber = Int32.TryParse(choice, out number);
                                count = 0;
                                foreach (var prod in products)
                                {
                                    count++;
                                    if (count == number || choice.ToLower() == prod.ProductName.ToLower())
                                    {
                                        valid = true;
                                        Console.WriteLine("Product ID: " + prod.ProductID + "\n" +
                                        "Product Name: " + prod.ProductName + "\n" +
                                        "Product Quantity Per Unit: " + prod.QuantityPerUnit + "\n" +
                                        "Product Unit Price: " + prod.UnitPrice + "\n" +
                                        "Product Units In Stock: " + prod.UnitsInStock + "\n" +
                                        "Product Units On Order: " + prod.UnitsOnOrder + "\n" +
                                        "Product Reorder Level: " + prod.ReorderLevel + "\n" +
                                        "Product Discontinued: " + prod.Discontinued + "\n" +
                                        "Product Category ID: " + prod.CategoryId + "\n" +
                                        "Product Supplier ID: " + prod.SupplierId);
                                    }
                                }

                                if (!valid)
                                {
                                    Console.WriteLine("Enter a valid product name or number");
                                }
                            } while (!valid);
                        }
                        break;
                    case "5":
                        //add category
                        Category category = new Category();

                        //check if that category already exists
                        isValid = false;
                        String categoryNameValidation = "";
                        while (isValid == false)
                        {
                            isValid = true;
                            Console.WriteLine("Enter Category Name:");
                            categoryNameValidation = Console.ReadLine();
                            foreach (var cat in categorys)
                            {
                                if (categoryNameValidation == cat.CategoryName)
                                {
                                    isValid = false;
                                    Console.WriteLine("Category already exists");
                                }
                            }
                        }
                        category.CategoryName = categoryNameValidation;

                        Console.WriteLine("Enter Category Description:");
                        category.Description = Console.ReadLine();

                        db.AddCategory(category);
                        logger.Info("Category added - {name}", categoryNameValidation);
                        break;
                    case "6":
                        //edit a category
                        count = 0;
                        foreach (var Cat in categorys)
                        {
                            Console.WriteLine(++count + ") " + Cat.CategoryName);
                        }
                        if (count == 0)
                        {
                            Console.WriteLine("No Categorys");
                        }
                        else {
                            Console.WriteLine(count + " Categorys returned");
                            Category categoryToEdit = new Category();
                            var valid = false;
                            while (!valid)
                            {
                                Console.WriteLine("Enter name or number");
                                choice = Console.ReadLine();

                                //check if string or number
                                number = 0;
                                stringOrNumber = Int32.TryParse(choice, out number);
                                count = 0;
                                foreach (var cat in categorys)
                                {
                                    count++;
                                    if (count == number || choice.ToLower() == cat.CategoryName.ToLower())
                                    {
                                        valid = true;
                                        categoryToEdit.CategoryId = cat.CategoryId;
                                        categoryToEdit.CategoryName = cat.CategoryName;
                                        categoryToEdit.Description = cat.Description;
                                    }
                                }
                            }

                            choice = "";
                            while (!(choice.ToLower() == "q"))
                            {
                                Console.WriteLine("Editing " + categoryToEdit.CategoryName + "\n");
                                Console.WriteLine("1) Edit Category Name");
                                Console.WriteLine("2) Edit Category Description");
                                Console.WriteLine("3) Edit Both");
                                Console.WriteLine("4) Delete Product");
                                Console.WriteLine("\"q\" to quit");
                                choice = Console.ReadLine();
                                logger.Info($"Option {choice} selected");

                                switch (choice)
                                {
                                    case "1":
                                        //check if that category already exists
                                        isValid = false;
                                        categoryNameValidation = "";
                                        while (isValid == false)
                                        {
                                            isValid = true;
                                            Console.WriteLine("Enter Category Name:");
                                            categoryNameValidation = Console.ReadLine();
                                            foreach (var cat in categorys)
                                            {
                                                if (categoryNameValidation == cat.CategoryName)
                                                {
                                                    isValid = false;
                                                    Console.WriteLine("Category already exists");
                                                }
                                            }
                                        }
                                        categoryToEdit.CategoryName = categoryNameValidation;
                                        db.EditCategory(categoryToEdit);
                                        logger.Info("Category edited - {name}", categoryToEdit.CategoryName);
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter New Category Description:");
                                        categoryToEdit.Description = Console.ReadLine();
                                        db.EditCategory(categoryToEdit);
                                        logger.Info("Category edited - {name}", categoryToEdit.CategoryName);
                                        break;
                                    case "3":
                                        //check if that category already exists
                                        isValid = false;
                                        categoryNameValidation = "";
                                        while (isValid == false)
                                        {
                                            isValid = true;
                                            Console.WriteLine("Enter Category Name:");
                                            categoryNameValidation = Console.ReadLine();
                                            foreach (var cat in categorys)
                                            {
                                                if (categoryNameValidation == cat.CategoryName)
                                                {
                                                    isValid = false;
                                                    Console.WriteLine("Category already exists");
                                                }
                                            }
                                        }
                                        categoryToEdit.CategoryName = categoryNameValidation;

                                        Console.WriteLine("Enter New Category Description:");
                                        categoryToEdit.Description = Console.ReadLine();
                                        db.EditCategory(categoryToEdit);
                                        logger.Info("Category edited - {name}", categoryToEdit.CategoryName);
                                        break;
                                    case "4":
                                        //delete category and all related products
                                        var productsToDelete = db.Products.Where(p => p.CategoryId == categoryToEdit.CategoryId);
                                        count = 0;
                                        foreach(var prod in productsToDelete)
                                        {
                                            Console.WriteLine(++count + ") "+prod.ProductName);
                                        }
                                        Console.WriteLine("Deleting the " +categoryToEdit.CategoryName +
                                            " means that you will also be deleting these products. Are you sure (Y/N)");
                                        string areYouSure = Console.ReadLine();
                                        if(areYouSure.ToLower() == "y" || areYouSure.ToLower() == "yes")
                                        {
                                            foreach (var prod in productsToDelete)
                                            {
                                                db.DeleteProduct(prod);
                                                logger.Info("Product deleted - {name}", prod.ProductName);
                                            }
                                            db.DeleteCategory(categoryToEdit);
                                            logger.Info("Category deleted - {name}", categoryToEdit.CategoryName);
                                            choice = "q";
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case "7":
                        count = 0;
                        foreach (var cat in categorys)
                        {
                            Console.WriteLine(++count + " Name: " + cat.CategoryName + "\nDescription: " + cat.Description + "\n");
                        }
                        break;
                    case "8":
                        count = 0;
                        foreach (var cat in categorys)
                        {
                            count++;
                        }

                        var catID = 0;
                        for (var i = 0; i < count; i++)
                        {
                            count = 0;
                            foreach (var cat in categorys)
                            {
                                if (i == count)
                                {
                                    catID = cat.CategoryId;
                                    Console.WriteLine(i + 1 + ") " + cat.CategoryName);
                                }
                                count++;
                            }
                            var productsInCategory = products.Where(p => p.CategoryId == (catID) && p.Discontinued == false);
                            var hasProducts = false;
                            foreach (var prod in productsInCategory)
                            {
                                hasProducts = true;
                                Console.WriteLine("\t" + prod.ProductName);
                            }

                            if (!hasProducts)
                            {
                                Console.WriteLine("\t0 products");
                            }
                            Console.WriteLine();
                        }
                        break;
                    case "9":
                        isValid = false;
                        catID = 0;
                        while (!isValid)
                        {
                            foreach (var cat in categorys)
                            {
                                Console.WriteLine(++count + ") " + cat.CategoryName);
                            }
                            Console.WriteLine("Enter a category to display(name or number)");
                            choice = Console.ReadLine();

                            //check if string or number
                            number = 0;
                            stringOrNumber = Int32.TryParse(choice, out number);
                            count = 0;
                            foreach (var cat in categorys)
                            {
                                count++;
                                if (count == number || choice.ToLower() == cat.CategoryName.ToLower())
                                {
                                    isValid = true;
                                    catID = cat.CategoryId;
                                }
                            }
                        }

                        count = 0;
                        var catPosition = 0;
                        foreach (var cat in categorys)
                        {
                            if (cat.CategoryId == catID)
                            {
                                catPosition = count;
                            }
                            count++;
                        }

                        count = 0;
                        foreach (var cat in categorys)
                        {
                            if (count == catPosition)
                            {
                                catID = cat.CategoryId;
                                Console.WriteLine(count + 1 + ") " + cat.CategoryName);
                            }
                            count++;
                        }
                        var productsInCategory2 = products.Where(p => p.CategoryId == (catID) && p.Discontinued == false);
                        var hasProducts2 = false;
                        foreach (var prod in productsInCategory2)
                        {
                            hasProducts2 = true;
                            Console.WriteLine("\t" + prod.ProductName);
                        }

                        if (!hasProducts2)
                        {
                            Console.WriteLine("\t0 products");
                        }
                        Console.WriteLine();

                        break;
                }

            } while (!(choice.ToLower() == "q"));
        }
    }
}
