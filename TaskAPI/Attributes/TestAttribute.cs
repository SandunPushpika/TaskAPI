using System.Reflection;

namespace TaskAPI.Web.Attributes {

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TestAttribute : Attribute{

        public string Name;
        public int Age { get; set; }
        public string Description;

        public TestAttribute(string Name) {
            this.Name = Name;
        }

        public static void DisplayAttribute(Type t) {

            MethodInfo[] methods = t.GetMethods();

            for (int i = 0; i < methods.Length; i++) {
                var att = Attribute.GetCustomAttribute(methods[i], typeof(TestAttribute)) as TestAttribute;
                if (att != null) {
                    Console.WriteLine(att.Name);
                    Console.WriteLine(att.Age);
                }
            }

        }

    }
}
