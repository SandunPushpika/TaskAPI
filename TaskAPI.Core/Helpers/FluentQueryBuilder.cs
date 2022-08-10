namespace TaskAPI.Core.Helpers {
    public class FluentQueryBuilder {

        private string query = "";

        public FluentQueryBuilder Select(string what) {
            query = query + "SELECT " + what+" ";
            return this;
        }

        public FluentQueryBuilder SelectAll() {
            query = query + "SELECT * ";
            return this;
        }

        public FluentQueryBuilder Add(Boolean brackets, params string[] values) {
            if (brackets) {
                query = query + "(" + GetValuesInOneLine(values) + ") ";
            } else {
                query = query + GetValuesInOneLine(values) + " ";
            }
            return this;
        }

        public FluentQueryBuilder From(string from) {
            query = query + "FROM "+ from+" ";
            return this;
        }

        public FluentQueryBuilder Where(string column, string value, string operation) {
            query = query + "WHERE "+ column +" "+ operation +" "+ value+" ";
            return this;
        }

        public FluentQueryBuilder Condition(string condition) {
            query = query + condition +" ";
            return this;
        }

        public FluentQueryBuilder InsertInto(string table) {
            query = query + "INSERT INTO " + table + " ";
            return this;
        }

        public FluentQueryBuilder Values(params string[] values) {

            query = query + "VALUES ("+ GetValuesInOneLine(values)+") ";

            return this;
        }

        public FluentQueryBuilder Update(string table) {
            query = query + "UPDATE "+table+" SET ";
            return this;
        }

        public FluentQueryBuilder Delete() {
            query = query + "DELETE ";
            return this;
        }

        private string GetValuesInOneLine(string[] values) {

            string line = "";

            for (int i = 0; i < values.Length; i++) {
                if (i == (values.Length - 1)) {
                    line = line + values[i];
                } else {
                    line = line + values[i] + ", ";
                }
            }

            return line;
        }

        public string Build() {
            return query;
        }

    }
}
