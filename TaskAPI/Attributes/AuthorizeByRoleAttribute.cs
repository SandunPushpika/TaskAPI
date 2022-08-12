using Microsoft.AspNetCore.Authorization;

namespace TaskAPI.Web.Attributes {
    public class AuthorizeByRoleAttribute : AuthorizeAttribute {

        public AuthorizeByRoleAttribute(params string[] roles) {
            Roles = String.Join(",", roles);
        }

    }
}
