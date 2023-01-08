using System;
using System.IO;

namespace Client {
    internal class Validator {
        private Validator() {
        }

        // 2000-01-01
        public static bool isValidDate(string date) {
            if (date.Length != 10) {
                Console.WriteLine("not a valid date! (must be yyyy-mm-dd)");
                return false;
            }
            for (int i = 0; i < date.Length; i++) {
                if (i == 4 || i == 7) {
                    if (date[i] != '-') {
                        Console.WriteLine("not a valid date! (must be yyyy-mm-dd)");
                        return false;
                    }
                } else {
                    if (!Char.IsDigit(date[i])) {
                        Console.WriteLine("not a valid date! (must be yyyy-mm-dd)");
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool isValidCaption(string caption) {
            if (String.IsNullOrEmpty(caption)) {
                Console.WriteLine("not a valid caption! (must not be blank or empty)");
                return false;
            }
            return true;
        }

        public static bool isValidFilePath(string filePath) {
            if (!File.Exists(filePath)) {
                Console.WriteLine("not a valid file path! (must be a path to a file that exists)");
                return false;
            }
            return true;
        }
    }
}
