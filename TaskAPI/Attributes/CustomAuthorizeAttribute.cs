using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskAPI.Web.Attributes {
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter {
        public int Age { get; set; }
        public CustomAuthorizeAttribute(int age) {
            Age = age;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            var age = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Age").Value;

            Console.WriteLine("Done");

            if (age != null && int.Parse(age) < Age) {
                Console.WriteLine("Passes");
                context.Result = new UnauthorizedResult();
            }
            
        }
    }
}
