using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Components
{
    public class PaymentDeliveryMethodViewComponent : ViewComponent
    {
        public List<DeliveryMethod> NavbarTop()
        {
            var topNav = new List<DeliveryMethod>();
            topNav.Add(new DeliveryMethod()
            {
                Name = "Odbiór w salonie",
                DeliveryMethodId = 1,
                PaymentMethods = new List<PaymentMethod>()
                {
                    new PaymentMethod() {PaymentMethodId = 1, Name = "Gotówka", Price = 0},
                    new PaymentMethod() {PaymentMethodId = 2, Name = "Karta", Price = 0},
                    new PaymentMethod() {PaymentMethodId = 3, Name = "Przelew bankowy", Price= 0},
                    new PaymentMethod() {PaymentMethodId = 4, Name = "Paypal", Price = 0}
                }
            });
            topNav.Add(
                new DeliveryMethod()
                {
                    Name = "Poczta Polska",
                    DeliveryMethodId = 2,

                    PaymentMethods = new List<PaymentMethod>()
                    {
                        new PaymentMethod() {PaymentMethodId = 5, Name="Opłata za pobraniem", Price = 15},
                        new PaymentMethod() {PaymentMethodId = 6, Name="Przelew bankowy", Price = 12},
                        new PaymentMethod() {PaymentMethodId = 7, Name="Paypal", Price = 12}
                    }
                });

            topNav.Add(
                new DeliveryMethod()
                {
                    Name = "Przesyłka kurierska",
                    DeliveryMethodId = 3,
                    PaymentMethods = new List<PaymentMethod>()
                    {
                        new PaymentMethod() {PaymentMethodId = 8, Name="Opłata za pobraniem", Price = 20},
                        new PaymentMethod() {PaymentMethodId = 9, Name="Przelew bankowy", Price = 15},
                        new PaymentMethod() {PaymentMethodId = 10, Name="Paypal", Price = 15}
                    }
                });
           
            return topNav;
        }
        
        public IViewComponentResult Invoke()
        {

            var data = NavbarTop();
            return View(data);
        }
    }
}