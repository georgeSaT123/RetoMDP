using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RetoMDPFINAL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Estructura del Tablero de Control
            string cuadroTotalSuma = "";
            // Estructura del Teclado a mosrar
            string teclado = "[ Q ]" + "[ W ]" + "[ E ]" + "[ R ]" + "[ T ]" + "[ Y ]" + "[ U ]" + "[ I ]" + "[ O ]" + "[ P ]" + "\n" +
                              "[ A ]" + "[ S ]" + "[ D ]" + "[ F ]" + "[ G ]" + "[ H ]" + "[ J ]" + "[ K ]" + "[ L ]" + "[ Ñ ]" + "\n" +
                              "\t" + "[ Z ]" + "[ X ]" + "[ C ]" + "[ V ]" + "[ B ]" + "[ N ]" + "[ M ]" + "\t";
            //CADENA DE LA RUTA DEL ARCHIVO
            string ruta = @"C:\Users\georg\Escritorio\dotNET-MDP\palabras5.txt";

            string _path = @"C:\Users\georg\Escritorio\dotNET-MDP\palabras_por_fecha.json";

            //Mostramos el Formato de Inicio
            metodos.CabeceraJuego();
            metodos.Formato(null, null, null, null, null, null);
            metodos.FormatoTeclado(teclado);

            //Se lee el achivo alabras5.txt y se guardan los datos en un array "Arreglo Palabras"
            string[] ArregloPalabras = metodos.LeerArchivo(ruta);

            //Se escoge una palabra aleatoria del array "ArregloPalabras"
            var palabraEscogida = metodos.EscogerPalabra(ArregloPalabras);
            //Console.WriteLine(palabraEscogida);

            //Se Quitan las Tildes a la palabra Escogida
            palabraEscogida = metodos.QuitarTildes(palabraEscogida);
            //Console.WriteLine(palabraEscogida);

            //Se hace toda palabra Mayuscula
            palabraEscogida = metodos.palabraMayuscula(palabraEscogida);

            //metodos de fecha
            DateTime fecha = metodos.ElegirFecha();

            Console.WriteLine(fecha);
            var Ver = metodos.RegistrarInformacionJSON(fecha, palabraEscogida);

            metodos.SerializeJsonFile(Ver,_path);           

            //Tenemos Todo OK
            //Iniciamos Juego

            bool IntentoOk = false;
            int IntentosUsados;
            const int IntentosMaximos = 6;
            for (IntentosUsados = 1; IntentosUsados <= IntentosMaximos; IntentosUsados++)
            {
                do
                {
                    
                    //Ingresamos Palabra
                    var palabraIngresada = metodos.IngresarPalabra();
                    //Quitamos Tildes o Espacios si Hay
                    palabraIngresada = metodos.QuitarTildes(palabraIngresada);
                    //Hacemos que la palabra sea todo en Mayuscula
                    palabraIngresada = metodos.palabraMayuscula(palabraIngresada);

                    if (palabraIngresada.Length == palabraEscogida.Length && metodos.VerificarSoloLetras(palabraIngresada) == true)
                    {
                        IntentoOk = true;

                        string[] letraCaso1 = new string[5];
                        string[] letraCaso2 = new string[5];
                        string[] letraCaso3 = new string[5];
                        
                        //Vemos los posibles casos de la palabra ingresa
                        /*
                        si la letra de la palabra Ingresada concide en valor y poscion con la letra
                        de la palabra Escogida, se guarda en el array caso 3
                        sino si: la letra de la palabra Ingresada no coincide en valor respecto a la posicion i con la
                        letra de la palabra Escogida, pero
                        1. Si se encuentra contenida en la Palabra Escogida, se guarda en el array caso 2
                        2. No se encuentra contenida, se guarda en el array caso1
                         */
                        for (int i = 0; i < palabraIngresada.Length; i++)
                        {                            
                                if (palabraIngresada.Substring(i, 1) == palabraEscogida.Substring(i, 1))
                                {
                                    letraCaso3[i] = palabraIngresada.Substring(i, 1);                                    
                                }
                                else if (palabraIngresada.Substring(i, 1) != palabraEscogida.Substring(i, 1))
                                {                                    
                                    if (palabraEscogida.Contains(palabraIngresada.Substring(i, 1)))
                                    {
                                    letraCaso2[i] = palabraIngresada.Substring(i, 1);
                                    }
                                    else
                                    {
                                    letraCaso1[i] = palabraIngresada.Substring(i, 1);
                                    }
                                }
                        }
                        //ingresamos los valores tanto en TABLA como en el TECLADO que se muestra
                        string[] letraIntentoI = new string[5];
                        for (int i = 0; i < 5; i++)
                        {
                            if (letraCaso1[i] != null)
                            {
                                letraIntentoI[i] = $">{letraCaso1[i]}<";
                                teclado = teclado.Replace($" {letraCaso1[i]} ", letraIntentoI[i]);

                            }
                            else if (letraCaso2[i] != null)
                            {                                
                                letraIntentoI[i] = $"<{letraCaso2[i]}>";                                
                                teclado = teclado.Replace($" {letraCaso2[i]} ", letraIntentoI[i]);
                                if(i>0)
                                    teclado = teclado.Replace($">{letraCaso2[i]}<", letraIntentoI[i]);
                            }
                            else if (letraCaso3[i] != null)
                            {
                                letraIntentoI[i] = $"={letraCaso3[i]}=";                                
                                teclado = teclado.Replace($" {letraCaso3[i]} ", letraIntentoI[i]);
                                if (i>0)
                                {
                                    teclado = teclado.Replace($">{letraCaso3[i]}<", letraIntentoI[i]);
                                    teclado = teclado.Replace($"<{letraCaso3[i]}>", letraIntentoI[i]);
                                }                                
                            }                            
                        }


                        string[] cuadroTotal = new string[6];

                        for (int i = 0; i < 6; i++)
                        {
                            if (IntentosUsados == i + 1)
                            {
                                cuadroTotal[i] = "+---+---+---+---+---+" + "\n" +
                                "|" + $"{letraIntentoI[0]}" +
                                "|" + $"{letraIntentoI[1]}" +
                                "|" + $"{letraIntentoI[2]}" +
                                "|" + $"{letraIntentoI[3]}" +
                                "|" + $"{letraIntentoI[4]}" + "|";
                                cuadroTotalSuma = cuadroTotalSuma + "\n" + cuadroTotal[i];
                            }

                        }

                        Console.WriteLine(cuadroTotalSuma);
                        if (IntentosUsados == 1)
                        {
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");                           
                        }
                        else if (IntentosUsados == 2)
                        {
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                        }
                        else if (IntentosUsados == 3)
                        {
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                        }
                        else if (IntentosUsados == 4)
                        {
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                        }
                        else if (IntentosUsados == 5)
                        {
                            Console.WriteLine("+---+---+---+---+---+");
                            Console.WriteLine("|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|" + "   " + "|");
                            Console.WriteLine("+---+---+---+---+---+");
                        }
                        else
                        {
                            Console.WriteLine("+---+---+---+---+---+");
                        }
                        metodos.FormatoTeclado(teclado);


                        //for esta enfocado el el String Acumulado del Teclado y si encuentra las letras=>  =(letras)= finaliza
                        for (int i=0; i<6; i++)
                        {                          
                            string letra1 = palabraIngresada.Substring(0, 1);                            
                            string letra2 = palabraIngresada.Substring(1, 1);
                            string letra3 = palabraIngresada.Substring(2, 1);
                            string letra4 = palabraIngresada.Substring(3, 1);
                            string letra5 = palabraIngresada.Substring(4, 1);
                            if (teclado.Contains($"={letra1}=") && teclado.Contains($"={letra2}=") && teclado.Contains($"={letra3}=") &&
                                teclado.Contains($"={letra4}=") && teclado.Contains($"={letra5}=")) {
                                Console.WriteLine("GANASTE");
                                Console.WriteLine($"PALABRA ADIVINADA {palabraEscogida}");
                                IntentosUsados = IntentosMaximos;
                                break;
                            }                         
                        }
                    }
                    

                    else
                    {
                        IntentoOk = false;
                        Console.WriteLine("xxxxxx INTENTO NO VALIDO xxxxxx");
                    }                    ;
                }
                while (IntentoOk != true);
            }
        }

        public static class metodos
        {
            //Metodo para leer los arthivos en un Array
            public static string[] LeerArchivo(string ruta)
            {
                string[] lines = System.IO.File.ReadAllLines(@$"{ruta}");
                return lines;
            }
            //Metodo para Escoger una Palabra Aleatoria
            public static string EscogerPalabra(string[] palabras)
            {
                Random rnd = new Random();
                int indice = rnd.Next(palabras.Length);
                var palabraAdivinada = palabras[indice];
                return palabraAdivinada;
            }
            //Metodo para Quitar Tildes y Espacio
            public static string QuitarTildes(string palabra)
            {
                Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
                Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
                Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
                Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
                Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
                Regex esp = new Regex(" ", RegexOptions.Compiled);
                palabra = a.Replace(palabra, "a");
                palabra = e.Replace(palabra, "e");
                palabra = i.Replace(palabra, "i");
                palabra = o.Replace(palabra, "o");
                palabra = u.Replace(palabra, "u");
                palabra = esp.Replace(palabra, "");
                return palabra;
            }
            //Metodo para verificar si solo se ingresan Letras
            public static bool VerificarSoloLetras(string palabra)
            {
                return palabra.All(char.IsLetter);
            }

            //Metodo para poner la palabra en mayuscula
            public static string palabraMayuscula(string palabra)
            {
                return palabra.ToUpper();
            }
            //3 Metodos para la Estructura del Juego sin Intento o Desde 1 hasta el Intento 6
            public static void CabeceraJuego()
            {
                Console.WriteLine("\t**************************************************************************************************\t");
                Console.WriteLine("\t\t\t\t\t\t\t" + "RETO MDP" + "\t\t\t\t\t\t\t");
                Console.WriteLine("\tMaximo 6 intentos para Ganar\t\tLetras de la palabra coinciden\t\tAdivina la palabra");
                Console.WriteLine("\t**************************************************************************************************\t");

            }
            // Ingresamos el formato para cuando es nulo llena todo en blanco, si se usan intentos vamos llenando fila por fila
            // segun el caso
            public static void Formato(string? letra1, string? letra2, string? letra3, string? letra4, string? letra5, int? intento)
            {
                Console.WriteLine("+---+---+---+---+---+");
                for (int i = 0; i < 6; i++)
                {
                    if (letra1 == null && letra2 == null && letra3 == null && letra4 == null && letra5 == null)
                    {
                        letra1 = " ";
                        letra2 = " ";
                        letra3 = " ";
                        letra4 = " ";
                        letra5 = " ";
                        Console.WriteLine("|" + $" {letra1} " + "|" + $" {letra2} " + "|" + $" {letra3} " + "|" + $" {letra4} " + "|" + $" {letra5} " + "|");
                        Console.WriteLine("+---+---+---+---+---+");
                    }

                    else
                    {
                        if (intento == 1)
                        {
                            if (i == 0)
                            {
                                Console.WriteLine("|" + $"{letra1}" + "|" + $"{letra2}" + "|" + $"{letra3}" + "|" + $"{letra4}" + "|" + $"{letra5}" + "|");
                                Console.WriteLine("+---+---+---+---+---+");
                            }
                        }
                        if (intento == 2)
                        {
                            if (i == 1)
                            {
                                Console.WriteLine("|" + $"{letra1}" + "|" + $"{letra2}" + "|" + $"{letra3}" + "|" + $"{letra4}" + "|" + $"{letra5}" + "|");
                                Console.WriteLine("+---+---+---+---+---+");
                            }
                        }
                        if (intento == 3)
                        {
                            if (i == 2)
                            {
                                Console.WriteLine("|" + $"{letra1}" + "|" + $"{letra2}" + "|" + $"{letra3}" + "|" + $"{letra4}" + "|" + $"{letra5}" + "|");
                                Console.WriteLine("+---+---+---+---+---+");
                            }
                        }
                        if (intento == 4)
                        {
                            if (i == 3)
                            {
                                Console.WriteLine("|" + $"{letra1}" + "|" + $"{letra2}" + "|" + $"{letra3}" + "|" + $"{letra4}" + "|" + $"{letra5}" + "|");
                                Console.WriteLine("+---+---+---+---+---+");
                            }
                        }
                        if (intento == 5)
                        {
                            if (i == 4)
                            {
                                Console.WriteLine("|" + $"{letra1}" + "|" + $"{letra2}" + "|" + $"{letra3}" + "|" + $"{letra4}" + "|" + $"{letra5}" + "|");
                                Console.WriteLine("+---+---+---+---+---+");
                            }
                        }
                        if (intento == 6)
                        {
                            if (i == 5)
                            {
                                Console.WriteLine("|" + $"{letra1}" + "|" + $"{letra2}" + "|" + $"{letra3}" + "|" + $"{letra4}" + "|" + $"{letra5}" + "|");
                                Console.WriteLine("+---+---+---+---+---+");
                            }
                        }
                        letra1 = " ";
                        letra2 = " ";
                        letra3 = " ";
                        letra4 = " ";
                        letra5 = " ";
                        Console.WriteLine("|" + $" {letra1} " + "|" + $" {letra2} " + "|" + $" {letra3} " + "|" + $" {letra4} " + "|" + $" {letra5} " + "|");
                        Console.WriteLine("+---+---+---+---+---+");
                    }
                }
            }
            //Parte de Teclado de la presentacion
            public static void FormatoTeclado(string teclado)
            {                
                Console.WriteLine("\n\n");
                Console.WriteLine(teclado);
            }
            //Metodo para Ingresar Una Palabra
            public static string IngresarPalabra()
            {
                Console.WriteLine("----- INGRESE UNA PALABRA -----");
                String palabra;
                palabra = Console.ReadLine();
                return palabra;
            }
            //Insertar o elegir Fecha
            public static DateTime ElegirFecha()
            {
                Console.WriteLine("Ingresa Fecha en formato YYYY-MM-DD");
                string fecha = Console.ReadLine();
                DateTime fechaDate = DateTime.ParseExact(fecha, "yyyy-MM-dd", null);
                return fechaDate;        
            } 
            //Generar el Diccionario {"FECHA" : "PALABRA"}
            public static Dictionary<DateTime, string> RegistrarInformacionJSON(DateTime fecha, string palabra) {
                Dictionary<DateTime, string> Informacion = new Dictionary<DateTime, string>()
                {
                    {fecha, palabra}
                };
                return Informacion;
            }
            //Serializar, pero lo convierto a un array
            public static void SerializeJsonFile(Dictionary<DateTime, string> valor, string path)
            {
                string valorJson = JsonConvert.SerializeObject(valor.ToArray(), Formatting.Indented);
                File.WriteAllText(path, valorJson);
            }
        }
    }
}