using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cripto1
{
    public class Program
    {

        public static void create_key(string scriptName, string key_file_path, string PythonDLLDirectory, string PythonScriptDirectory)
        {
            if (!PythonEngine.IsInitialized)
            {
                Runtime.PythonDLL = PythonDLLDirectory;
                PythonEngine.Initialize();
            }
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(PythonScriptDirectory);

                var pythonScript = Py.Import(scriptName);
                var key_file_path_object = new PyString(key_file_path);
                pythonScript.InvokeMethod("create_key", new PyObject[] { key_file_path_object });
            }
        }

        public static void encrypt_excel_file_with_key(string scriptName, string input_file_path, string key_file_path, string PythonDLLDirectory, string PythonScriptDirectory)
        {
            if (!PythonEngine.IsInitialized)
            {
                Runtime.PythonDLL = PythonDLLDirectory;
                PythonEngine.Initialize();
            }
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(PythonScriptDirectory);

                var pythonScript = Py.Import(scriptName);
                var input_file_path_object = new PyString(input_file_path);
                var key_file_path_object = new PyString(key_file_path);
                pythonScript.InvokeMethod("encrypt_excel_file_with_key", new PyObject[] { input_file_path_object, key_file_path_object });
            }
        }

        public static void decript_excel_file_with_key(string scriptName, string input_file_path, string key_file_path, string PythonDLLDirectory, string PythonScriptDirectory)
        {
            if (!PythonEngine.IsInitialized)
            {
                Runtime.PythonDLL = PythonDLLDirectory;
                PythonEngine.Initialize();
            }
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(PythonScriptDirectory);

                var pythonScript = Py.Import(scriptName);
                var input_file_path_object = new PyString(input_file_path);
                var key_file_path_object = new PyString(key_file_path);
                pythonScript.InvokeMethod("decrypt_excel_file_with_key", new PyObject[] { input_file_path_object, key_file_path_object });
            }
        }

    }
}
