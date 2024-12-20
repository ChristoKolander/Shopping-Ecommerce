using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopping.Core.Entities;
using Shopping.Core.Entities.People;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Infrastructure.Data
{
    public class ProductContextSeed
    {
        public static async Task SeedAsync(ProductContext productContext,
           ILogger logger,
           int retry = 0)
        {

            var retryForAvailability = retry;
            try
            {
                if (productContext.Database.IsSqlServer())
                {
                    productContext.Database.Migrate();
                }

                if (!await productContext.ProductCategories.AnyAsync())
                {
                    await productContext.ProductCategories.AddRangeAsync(
                       GetPreconfiguredProductCategories());

                    await productContext.SaveChangesAsync();
                }

                if (!await productContext.Products.AnyAsync())
                {
                    await productContext.Products.AddRangeAsync(
                        GetPreconfiguredProducts());

                    await productContext.SaveChangesAsync();
                }

                if (!await productContext.Customers.AnyAsync())
                {
                    await productContext.Customers.AddRangeAsync(
                        GetPreconfiguredCustsomers());

                    await productContext.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;

                logger.LogError(ex.Message);
                await SeedAsync(productContext, logger, retryForAvailability);
                throw;
            }
        }


        #region Seeding Categories and Products

        static IEnumerable<ProductCategory> GetPreconfiguredProductCategories()
        {
            return new List<ProductCategory>()
            {
            new ProductCategory(){Name = "Beauty", IconCSS = "fas fa-spa"},
            new ProductCategory(){Name = "Furniture", IconCSS = "fas fa-couch"},
            new ProductCategory(){Name = "Electronics", IconCSS = "fas fa-headphones"},
            new ProductCategory(){Name = "Shoes", IconCSS = "fas fa-shoe-prints"}
            };

        }

        static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>
            { 
				//Beauty
				new Product{
                Name = "Glossier - Beauty Kit",
                Description = "A kit provided by Glossier, containing skin care, hair care and makeup products",
                ImageURL = "/Images/Beauty/Beauty1.png",
                Price = 100,
                ProductCategoryId = 1
            },

                new Product{
                Name = "Curology - Skin Care Kit",
                Description = "A kit provided by Curology, containing skin care products",
                ImageURL = "/Images/Beauty/Beauty2.png",
                Price = 50,
                ProductCategoryId = 1
            },


                new Product{
                Name = "Cocooil - Organic Coconut Oil",
                Description = "A kit provided by Curology, containing skin care products",
                ImageURL = "/Images/Beauty/Beauty3.png",
                Price = 20,
                ProductCategoryId = 1
            },


                new Product{
                Name = "Schwarzkopf - Hair Care and Skin Care Kit",
                Description = "A kit provided by Schwarzkopf, containing skin care and hair care products",
                ImageURL = "/Images/Beauty/Beauty4.png",
                Price = 50,
                ProductCategoryId = 1
            },

                new Product{
                Name = "Skin Care Kit",
                Description = "Skin Care Kit, containing skin care and hair care products",
                ImageURL = "/Images/Beauty/Beauty5.png",
                Price = 30,
                ProductCategoryId = 1
            },


			//Electronics Category
				new Product{
                Name = "Air Pods",
                Description = "Air Pods - in-ear wireless headphones",
                ImageURL = "/Images/Electronic/Electronics1.png",
                Price = 100,
                ProductCategoryId = 3
            },


                new Product{
                Name = "On-ear Golden Headphones",
                Description = "On-ear Golden Headphones - these headphones are not wireless",
                ImageURL = "/Images/Electronic/Electronics2.png",
                Price = 40,
                ProductCategoryId = 3
            },

                new Product{
                Name = "On-ear Black Headphones",
                Description = "On-ear Black Headphones - these headphones are not wireless",
                ImageURL = "/Images/Electronic/Electronics3.png",
                Price = 40,
                ProductCategoryId = 3
            },

                new Product{
                Name = "Sennheiser Digital Camera with Tripod",
                Description = "Sennheiser Digital Camera - High quality digital camera provided by Sennheiser - includes tripod",
                ImageURL = "/Images/Electronic/Electronic4.png",
                Price = 600,
                ProductCategoryId = 3
            },


                new Product{
                Name = "Canon Digital Camera",
                Description = "Canon Digital Camera - High quality digital camera provided by Canon",
                ImageURL = "/Images/Electronic/Electronic5.png",
                Price = 500,
                ProductCategoryId = 3
            },

                new Product{
                Name = "Nintendo Gameboy",
                Description = "Gameboy - Provided by Nintendo",
                ImageURL = "/Images/Electronic/technology6.png",
                Price = 100,
                ProductCategoryId= 3
            },

			//Furniture Category
				new Product{
                Name = "Black Leather Office Chair",
                Description = "Very comfortable black leather office chair",
                ImageURL = "/Images/Furniture/Furniture1.png",
                Price = 50,
                ProductCategoryId = 2
            },

                new Product{
                Name = "Pink Leather Office Chair",
                Description = "Very comfortable pink leather office chair",
                ImageURL = "/Images/Furniture/Furniture2.png",
                Price = 50,
                ProductCategoryId = 2
            },

                new Product{
                Name = "Lounge Chair",
                Description = "Very comfortable lounge chair",
                ImageURL = "/Images/Furniture/Furniture3.png",
                Price = 70,
                ProductCategoryId = 2
            },

                new Product{
                Name = "Silver Lounge Chair",
                Description = "Very comfortable Silver lounge chair",
                ImageURL = "/Images/Furniture/Furniture4.png",
                Price = 120,
                ProductCategoryId = 2
            },
                new Product{
                Name = "Porcelain Table Lamp",
                Description = "White and blue Porcelain Table Lamp",
                ImageURL = "/Images/Furniture/Furniture6.png",
                Price = 15,
                ProductCategoryId = 2
            },

                new Product{
                Name = "Office Table Lamp",
                Description = "Office Table Lamp",
                ImageURL = "/Images/Furniture/Furniture7.png",
                Price = 20,
                ProductCategoryId = 2
            },

			//Shoes Category
				new Product{
                Name = "Puma Sneakers",
                Description = "Comfortable Puma Sneakers in most sizes",
                ImageURL = "/Images/Shoes/Shoes1.png",
                Price = 100,
                ProductCategoryId = 4

            },

                new Product{
                Name = "Colorful Trainers",
                Description = "Colorful trainsers - available in most sizes",
                ImageURL = "/Images/Shoes/Shoes2.png",
                Price = 150,
                ProductCategoryId = 4
            },
                new Product{
                Name = "Blue Nike Trainers",
                Description = "Blue Nike Trainers - available in most sizes",
                ImageURL = "/Images/Shoes/Shoes3.png",
                Price = 200,
                ProductCategoryId = 4
            },

                new Product{
                Name = "Colorful Hummel Trainers",
                Description = "Colorful Hummel Trainers - available in most sizes",
                ImageURL = "/Images/Shoes/Shoes4.png",
                Price = 120,
                ProductCategoryId = 4
            },

                new Product{
                Name = "Red Nike Trainers",
                Description = "Red Nike Trainers - available in most sizes",
                ImageURL = "/Images/Shoes/Shoes5.png",
                Price = 200,
                ProductCategoryId = 4
            },

                new Product{
                Name = "Birkenstock Sandles",
                Description = "Birkenstock Sandles - available in most sizes",
                ImageURL = "/Images/Shoes/Shoes6.png",
                Price = 50,
                ProductCategoryId = 4
                }
            };

        }
        
        #endregion


        #region Seeding Customers

        static IEnumerable<Customer> GetPreconfiguredCustsomers()
        {
            return new List<Customer>
            {

                new Customer
                {

                    LastName = "Johansen",
                    FirstName = "Bibi",
                    Title = "Miss",
                    MemberSince = new DateOnly(2014, 4, 1),
                    Visits = new List<DateOnly>
                    {
                        new DateOnly(2022, 1, 1),
                        new DateOnly(2024, 11, 1),
                        new DateOnly(2018, 7, 1)
                    },
                    Details = new CustomerDetails
                    {
                        Region = "North America",
                        Notes = new List<string>
                            {
                                "SalesWoman from Wyoming",
                                " Married with Children",
                                " Highly Educated, Stanford"
                            },
                        Addresses =
                        {
                            new Address
                            {

                                Country = "USA",
                                State = "California",
                                City = "Los Angeles",
                                Street = "Walking Street",
                                Zipcode = "123 45",
                                IsPrimary = true
                            },

                            {
                                new Address
                                {
                                    Country = "USA",
                                    State = "New York",
                                    City = "New York",
                                    Street = "Broadway",
                                    Zipcode = "543 21",
                                    IsPrimary = false
                                }
                            }
                        },
                        PhoneNumbers =
                            {
                                new Phone
                            {
                                CountryCode = 46,
                                PhoneNumber = "12 34 56",
                                IsPrimary = true
                            },
                                new Phone
                            {
                                CountryCode = 46,
                                PhoneNumber = "56 22 34",
                                IsPrimary = false
                            },

                        }
                    }

                },

                new Customer
                {

                    LastName = "First",
                    FirstName = "Peter",
                    Title = "Mr",
                    MemberSince = new DateOnly(2012, 10, 10),
                    Visits = new List<DateOnly>
                    {
                        new DateOnly(2024, 1, 1),
                        new DateOnly(2023, 11, 1),
                        new DateOnly(2019, 7, 1)
                    },
                    Details = new CustomerDetails
                    {
                        Region = "North America",
                        Notes = new List<string>
                            {
                                "CEO from Dallas",
                                " Divorced, three children",
                                " YMCA"
                            },
                        Addresses =
                        {
                            new Address
                            {

                                Country = "USA",
                                State = "Texas",
                                City = "Dallas",
                                Street = "Bourbon Street 22",
                                Zipcode = "656 22",
                                IsPrimary = true
                            },

                            {
                                new Address
                                {
                                    Country = "USA",
                                    State = "Fort Lauderdale",
                                    City = "Florida",
                                    Street = "Relaxation Road",
                                    Zipcode = "111 88",
                                    IsPrimary = false
                                }
                            }
                        },

                         PhoneNumbers =
                            {
                                new Phone
                            {
                                CountryCode = 33,
                                PhoneNumber = "12 34 56",
                                IsPrimary = true
                            },
                                new Phone
                            {
                                CountryCode = 46,
                                PhoneNumber = "56 22 34",
                                IsPrimary = false
                            },

                         }
                    }
                },


                new Customer
                {

                    LastName = "Second",
                    FirstName = "Peter",
                    Title = "Mr.",
                    MemberSince = new DateOnly(2012, 10, 10),
                    Visits = new List<DateOnly>
                    {
                        new DateOnly(2024, 1, 1),
                        new DateOnly(2023, 11, 1),
                        new DateOnly(2019, 7, 1)
                    },
                    Details = new CustomerDetails
                    {
                        Region = "North America",
                        Notes = new List<string>
                            {
                                "CEO from Dallas",
                                " Divorced, three children",
                                " YMCA"
                            },
                        Addresses =
                        {
                            new Address
                            {

                                Country = "USA",
                                State = "Texas",
                                City = "Dallas",
                                Street = "Bourbon Street 22",
                                Zipcode = "656 22",
                                IsPrimary = true
                            },

                            {
                                new Address
                                {
                                    Country = "USA",
                                    State = "Fort Lauderdale",
                                    City = "Florida",
                                    Street = "Relaxation Road",
                                    Zipcode = "111 88",
                                    IsPrimary = false
                                }
                            }
                        },
                         PhoneNumbers =
                            {
                                new Phone
                            {
                                CountryCode = 46,
                                PhoneNumber = "12 34 56",
                                IsPrimary = true
                            },
                                new Phone
                            {
                                CountryCode = 46,
                                PhoneNumber = "56 22 34",
                                IsPrimary = false
                            },
                        }
                    }

                },

                new Customer
                {

                    LastName = "Yin",
                    FirstName = "Yan",
                    Title = "Mr.",
                    MemberSince = new DateOnly(2012, 10, 10),
                    Visits = new List<DateOnly>
                    {
                        new DateOnly(2024, 1, 1),
                        new DateOnly(2023, 11, 1),
                        new DateOnly(2019, 7, 1)
                    },
                    Details = new CustomerDetails
                    {
                        Region = "East Asia",
                        Notes = new List<string>
                        {
                            "CEO from Tokyo",
                            " Married, no children",
                            " Tokyo University"
                        },
                        Addresses =
                        {
                            new Address
                            {

                                Country = "Japan",
                                State = "Ozaka",
                                City = "Tokyo",
                                Street = "Hing Hai Street",
                                Zipcode = "777 22",
                                IsPrimary = true
                            },

                            {
                                new Address
                                {
                                    Country = "England",
                                    State = "Wessex",
                                    City = "London",
                                    Street = "Main Street",
                                    Zipcode = "111 88",
                                    IsPrimary = false
                                }
                            }
                        },
                        PhoneNumbers =
                            {
                                new Phone
                            {
                                CountryCode = 11,
                                PhoneNumber = "12 34 56",
                                IsPrimary = true
                            },
                                new Phone
                            {
                                CountryCode = 11,
                                PhoneNumber = "56 22 34",
                                IsPrimary = false
                            },
                        }
                    }

                }
            };
        }
        
        #endregion
    }
}







 