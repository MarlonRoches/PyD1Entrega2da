using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nuevoProyecto.Models
{
    public class ArbolB : IComparable
    {
        public NodoB Raiz { get; set; }
        static Global objeto;
        public Dictionary<string,string> Variables = new Dictionary<string, string>();
        int ConT = 0;
        NodoB nModificador = new NodoB();
        NodoB nA_Dar = new NodoB();
        public ArbolB()
        {
            Raiz = null;
            
        }
        
        public Dictionary<string,string> Diccionario()
        {
            return Variables;
        }
        
        /// <summary>
        /// Verifica si el nodo actual tiene hijos
        /// </summary>
        /// <param name="Nodo">Recibe el nodo sobre el cual se va a verificar</param>
        /// <returns></returns>
        public bool Hay_HIjos(NodoB Nodo)
        {
            if (Nodo.Nodos.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Verifica si el nodo tiene espacios aun disponibles para insaertar
        /// </summary>
        /// <param name="Nodo">Recibe el nodo sobre el cual se va a verificar</param>
        /// <returns></returns>
        public bool Disponibilidad(NodoB Nodo)
        {
            if (Nodo.Datos.Count <= Nodo.Maximo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// inserta el nodo en el arbol y todos los subprocesos
        /// </summary>
        /// <param name="Nodo"></param>
        /// <param name="valor"></param>
        public void Insertar(NodoB Nodo, Global valor)
        {
            if (Raiz == null)
            {
                Raiz = new NodoB();
                Raiz.AsignarGrado(Raiz, 3);
                Raiz.Datos.Add(valor);
                Raiz.Id = 1;
                Nodo = Raiz;
            }
            //ES HOJA
            else if (Hay_HIjos(Nodo) == false)
            {
                AgregarYOrdenarNodo(valor, Nodo);
            }
            //NO ES HOJA
            else if (Hay_HIjos(Nodo) == true)
            {
                var NodoHijo = new NodoB();
                NodoHijo = Nodo.Nodos[PosicionHijo(Nodo, valor)]; //busca lugar
                Insertar(NodoHijo, valor);
            }

            if (Disponibilidad(Nodo) == false)
            {
                Separar(Nodo);
            }
        }

        /// <summary>
        /// Crea un nodo del mismo grado del arbol
        /// </summary>
        /// <param name="Recibido">Nodo REcibido para crear y asignar atriubutos</param>
        /// <param name="Base">Nodo base para tomar como referencia el valor</param>
        /// 
        public void CreandoNodo(NodoB Recibido, NodoB Base)
        {
            Recibido.Maximo = Base.Maximo;
            Recibido.Minimo = Base.Minimo;
            Recibido.Id = Base.Id++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Nodo"></param>
        public void Separar(NodoB Nodo)
        {
            NodoB izq = new NodoB();
            NodoB padreAux = new NodoB();
            NodoB der = new NodoB();
            CreandoNodo(izq, Nodo);
            CreandoNodo(der, Nodo);

            for (int i = 0; i < Nodo.Minimo; i++)
            {
                izq.Datos.Add(Nodo.Datos[i]);
            }

            for (int i = Nodo.Minimo + 1; i <= Nodo.Maximo; i++)
            {
                der.Datos.Add(Nodo.Datos[i]);
            }


            if (Nodo.Padre != null) //Si es cualquier hijo
            {
                PadreHijo(Nodo.Padre, izq);
                PadreHijo(Nodo.Padre, der);

                Nodo.Padre.Datos.Add(Nodo.Datos[Nodo.Minimo]);
                Nodo.Padre.Datos.Sort((x, y) => x.CompareTo(y));

                int indice = 0;

                for (int i = 0; i < Nodo.Padre.Nodos.Count; i++)
                {
                    if (Nodo.Padre.Nodos[i].Datos.Count > 4)
                    {
                        indice = i;
                        break;
                    }
                }

                if (Nodo.Nodos.Count > 0)
                {
                    HijosDeHijos(Nodo, izq, 0, Nodo.Minimo);
                    HijosDeHijos(Nodo, der, Nodo.Minimo + 1, Nodo.Maximo + 1);
                }

                Nodo.Padre.Nodos.RemoveAt(indice);
                Nodo.Padre.Nodos.Sort((x, y) => x.Datos[0].CompareTo(y.Datos[0]));
                Nodo = null;
            }//Si es la raiz y aun caben valores en el nodo
            else if (Nodo.Padre == null && Nodo.Nodos.Count < 5)
            {
                padreAux.Datos.Add(Nodo.Datos[Nodo.Minimo]);
                PadreHijo(Nodo, izq);
                PadreHijo(Nodo, der);
                Nodo.Datos.Sort((x, y) => x.CompareTo(y));
                Nodo.Datos = padreAux.Datos;
            }//Si es raiz y no caben valores
            else if (Nodo.Padre == null && Nodo.Nodos.Count >= 5)
            {
                Global val = Nodo.Datos[Nodo.Minimo];

                HijosDeHijos(Nodo, izq, 0, Nodo.Minimo);
                HijosDeHijos(Nodo, der, Nodo.Minimo + 1, Nodo.Maximo + 1);

                Nodo.Nodos.Clear();
                PadreHijo(Nodo, izq);
                PadreHijo(Nodo, der);

                Nodo.Datos.Clear();
                Nodo.Datos.Add(val);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Nodo"></param>
        /// <param name="hijo"></param>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        public void HijosDeHijos(NodoB Nodo, NodoB hijo, int inicio, int fin)
        {
            for (int i = inicio; i <= fin; i++)
            {
                hijo.Nodos.Add(Nodo.Nodos[i]);
            }
            foreach (var item in hijo.Nodos)
            {
                item.Padre = hijo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Padre"></param>
        /// <param name="Hijo"></param>
        public void PadreHijo(NodoB Padre, NodoB Hijo)
        {
            Padre.Nodos.Add(Hijo);
            Hijo.Padre = Padre;
        }

        /// <summary>
        /// Busca en el arbolo la posicion enla cual va a ser insertado el  nodo/valor
        /// </summary>
        /// <param name="Nodo">Nodod a insertar</param>
        /// <param name="valor">Objeto que el nodo contiene</param>
        /// <returns></returns>
        /// 
        public int PosicionHijo(NodoB Nodo, Global valor)
        {
            if (Nodo.Datos.Count == 1)
            {
                if (valor.CompareTo(Nodo.Datos[0]) < 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                for (int i = 0; i < Nodo.Datos.Count - 1; i++)
                {
                    if (valor.CompareTo(Nodo.Datos[i]) < 0)
                    {
                        return i;
                    }
                    else if (valor.CompareTo(Nodo.Datos[i]) > 0 && valor.CompareTo(Nodo.Datos[i + 1]) < 0)
                    {
                        return i + 1;
                    }
                }
                return Nodo.Datos.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="Nodo"></param>
        public void AgregarYOrdenarNodo(Global valor, NodoB Nodo)
        {
            Nodo.Datos.Add(valor);
            Nodo.Datos.Sort((x, y) => x.CompareTo(y));
        }

        /// <summary>
        /// Busca un nodo con un dato en especifico 
        /// </summary>
        /// <param name="valor">Dato a buscar</param>
        /// <param name="Nodo">Nodo en el cual empezar</param>
        /// <returns>objeto sugerido</returns>
        public Global Busqueda(Global valor, NodoB Nodo)
        {
            bool BEncontrado = false;
            foreach (var item in Nodo.Datos)
            {
                if (item.CompareTo(valor) == 0)
                {
                    BEncontrado = true;
                    objeto = item;
                    break;
                }
            }

            if (BEncontrado == false && Nodo.Nodos.Count > 0)
            {
                NodoB NodoHijo = new NodoB();
                NodoHijo = Nodo.Nodos[PosicionHijo(Nodo, valor)];
                return Busqueda(valor, NodoHijo);
            }
            else if (BEncontrado == true)
            {
                return objeto;
            }
            else if (Nodo.Nodos.Count == 0)
            {
                throw new NotImplementedException();
            }
            else
            {
                return objeto;
            }

        }

        /// <summary>
        /// Devuelve si el nodo se encuentra bajo el estado minimo de datos posibles dentro de el
        /// </summary>
        /// <param name="Nodo">Nodo a evaluar</param>
        /// <returns>Bool</returns>
        public bool ExisteUnderFlow(NodoB Nodo)
        {
            if (Nodo.Datos.Count < Nodo.Minimo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="Nodo"></param>
        public void Eliminar(Global valor, NodoB Nodo)
        {
            int indice = 0;
            bool NodoInicial = false;
            bool BEncontrado = false;
            for (int i = 0; i < Nodo.Datos.Count; i++)
            {
                if (Nodo.Datos[i].CompareTo(valor) == 0)
                {
                    BEncontrado = true;
                    if (Hay_HIjos(Nodo) == true)
                    {
                        //if (contador == 0)
                        //{
                        indice = i;  //INDICE DE VALOR A SUSTITUIR
                        nModificador = Nodo; //NODO A SUSTITUIR VALOR DE NODO MAS IZQUIERDO DEL HIJO DERECHO
                        ConT++; //revisar para que se usa
                        NodoInicial = true;
                        //}

                        nA_Dar = MasIzquierdoDeDerecho(indice, Nodo, false);

                        nModificador.Datos.Add(nA_Dar.Datos[0]);
                        nModificador.Datos.RemoveAt(indice);
                        nA_Dar.Datos.RemoveAt(0);

                        nModificador.Datos.Sort((x, y) => x.CompareTo(y));

                        if (ExisteUnderFlow(nA_Dar) == true)
                        {
                            //Llamo a un metodo para ver si un hermano VECINO puede prestar
                            VerificarHermanos(nA_Dar.Padre, nA_Dar);
                        }
                    }
                    else
                    {
                        Nodo.Datos.RemoveAt(i);
                    }

                    if (NodoInicial == true)
                    {
                        Nodo = nModificador;
                    }

                    if (ExisteUnderFlow(Nodo) == true)
                    {
                        //Llamo a un metodo para ver si un hermano VECINO puede prestar
                        VerificarHermanos(Nodo.Padre, Nodo);
                    }
                }
            }

            if (BEncontrado == false && Nodo.Nodos.Count > 0)
            {
                NodoB NodoHijo = new NodoB();
                NodoHijo = Nodo.Nodos[PosicionHijo(Nodo, valor)];
                Eliminar(valor, NodoHijo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="padre"></param>
        /// <param name="nodo"></param>
        public void VerificarHermanos(NodoB padre, NodoB nodo)
        {

            NodoB hermano = new NodoB();
            int posicionvalorpadre = 0;
            for (int i = 0; i < padre.Nodos.Count; i++)
            {   //Si nodo es el primero y el hermano a la derecha puede prestar
                if (padre.Nodos[i] == nodo && i == 0 && padre.Nodos[i + 1].Datos.Count > padre.Minimo)
                {
                    hermano = padre.Nodos[i + 1];
                    posicionvalorpadre = i;
                    break;
                }
                //Si nodo es el ultimo y el hermano a la izquierda puede prestar
                else if (padre.Nodos[i] == nodo && i == (padre.Nodos.Count - 1) && padre.Nodos[i - 1].Datos.Count > padre.Minimo)
                {
                    hermano = padre.Nodos[i - 1];
                    posicionvalorpadre = i - 1;
                    break;
                }
                //Si el nodo se encuentra en medio, y el hermano a prestar es el de la izquierda
                else if (padre.Nodos[i] == nodo && i > 0 && i < padre.Nodos.Count - 1 && padre.Nodos[i - 1].Datos.Count > padre.Minimo)
                {
                    hermano = padre.Nodos[i - 1];
                    posicionvalorpadre = i - 1;
                    break;
                }
                //Si el nodo se encuentra en medio, y el hermano a prestar es el de la derecha
                else if (padre.Nodos[i] == nodo && i > 0 && i < padre.Nodos.Count - 1 && padre.Nodos[i + 1].Datos.Count > padre.Minimo)
                {
                    hermano = padre.Nodos[i + 1];
                    posicionvalorpadre = i;
                    break;
                }

            }

            //Si no encontro hermano para prestar valor
            if (hermano.Padre == null)
            {
                for (int i = 0; i < padre.Nodos.Count; i++)
                {
                    //SI NO SE PUEDEN PRESTAR VALORES SOLO PASA LA POSICION DEL PADRE
                    //Si el nodo que tiene underflow es el primero 
                    if (padre.Nodos[i] == nodo && i == 0)
                    {
                        posicionvalorpadre = i;
                        hermano = padre.Nodos[posicionvalorpadre + 1];
                        break;
                    }
                    //Si el nodo que tiene underflow es el ultimo
                    else if (padre.Nodos[i] == nodo && i == (padre.Nodos.Count - 1))
                    {
                        posicionvalorpadre = i - 1;
                        hermano = padre.Nodos[posicionvalorpadre];
                        break;
                    }
                    //Si el nodo que tiene underflow esta en medio
                    else if (padre.Nodos[i] == nodo && i > 0 && i < padre.Nodos.Count - 1)
                    {
                        posicionvalorpadre = i;
                        if (padre.Nodos[posicionvalorpadre + 1].Datos.Count > padre.Nodos[posicionvalorpadre - 1].Datos.Count)
                        {
                            hermano = padre.Nodos[posicionvalorpadre + 1];
                        }
                        else if (padre.Nodos[posicionvalorpadre + 1].Datos.Count < padre.Nodos[posicionvalorpadre - 1].Datos.Count)
                        {
                            hermano = padre.Nodos[posicionvalorpadre - 1];
                        }
                        else
                        {
                            hermano = padre.Nodos[posicionvalorpadre + 1];
                        }
                        break;
                    }
                }

                JuntarNodos(padre, posicionvalorpadre, nodo, hermano);
            }
            //Encontro valor y solo se traslada
            else
            {
                TrasladarValor(hermano, posicionvalorpadre, padre, nodo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hijoEmisor"></param>
        /// <param name="posicionvalorpadre"></param>
        /// <param name="padre"></param>
        /// <param name="nodoReceptor"></param>
        public void TrasladarValor(NodoB hijoEmisor, int posicionvalorpadre, NodoB padre, NodoB nodoReceptor)
        {

            //NODO HERMANO IZQUIERDO: Si debo de pasar el ultimo dato
            if (hijoEmisor.Datos[hijoEmisor.Datos.Count - 1].CompareTo(padre.Datos[posicionvalorpadre]) < 0) //Hijo a la izquierda
            {
                padre.Datos.Add(hijoEmisor.Datos[hijoEmisor.Datos.Count - 1]); //Subir valor
                hijoEmisor.Datos.Remove(hijoEmisor.Datos[hijoEmisor.Datos.Count - 1]); //quita valor que se paso a padre
                nodoReceptor.Datos.Add(padre.Datos[posicionvalorpadre]); //bajar valor de padre
                padre.Datos.Remove(padre.Datos[posicionvalorpadre]);
                nodoReceptor.Datos.Sort((x, y) => x.CompareTo(y)); // ordenar hijo que recibe

                //hijoEmisor.Valores.RemoveAt(hijoEmisor.Valores.Count - 1);

                padre.Datos.Sort((x, y) => x.CompareTo(y));
            }

            //HIJO A LA DERECHA: Si debo de pasar el primer dato
            else if (hijoEmisor.Datos[0].CompareTo(padre.Datos[posicionvalorpadre]) > 0)
            {
                padre.Datos.Add(hijoEmisor.Datos[0]);
                nodoReceptor.Datos.Add(padre.Datos[posicionvalorpadre]);
                nodoReceptor.Datos.Sort((x, y) => x.CompareTo(y));

                hijoEmisor.Datos.Remove(hijoEmisor.Datos[0]);

                padre.Datos.Remove(padre.Datos[posicionvalorpadre]);

                padre.Datos.Sort((x, y) => x.CompareTo(y));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="padre"></param>
        /// <param name="posicionvalorpadre"></param>
        /// <param name="hijo"></param>
        /// <param name="hermano"></param>
        public void JuntarNodos(NodoB padre, int posicionvalorpadre, NodoB hijo, NodoB hermano)
        {

            if (hijo.Datos[0].CompareTo(padre.Datos[padre.Datos.Count - 1]) > 0) //HIJO A LA DERECHA: Si es el ultimo nodo
            {
                hermano = padre.Nodos[posicionvalorpadre];
                for (int i = 0; i < hermano.Datos.Count; i++)
                {
                    hijo.Datos.Add(hermano.Datos[i]);
                }

                if (Hay_HIjos(hijo) == true)
                {
                    for (int i = 0; i < hermano.Nodos.Count; i++)
                    {
                        hijo.Nodos.Add(hermano.Nodos[i]);
                    }
                }

                padre.Nodos.RemoveAt(posicionvalorpadre); //Elimina el nodo hermano
                hijo.Datos.Add(padre.Datos[posicionvalorpadre]); //Manda valor de raiz
                hijo.Datos.Sort((x, y) => x.CompareTo(y));

                padre.Datos.Remove(padre.Datos[posicionvalorpadre]);

                if (padre.Datos.Count < padre.Minimo)
                {
                    VerificarHermanos(padre.Padre, padre);
                }
            }

            else if (hijo.Datos[hijo.Datos.Count - 1].CompareTo(padre.Datos[0]) < 0) //HIJO A LA IZQUIERDA DEL PADRE
            {
                hermano = padre.Nodos[posicionvalorpadre + 1];
                for (int i = 0; i < hermano.Datos.Count; i++)
                {
                    hijo.Datos.Add(hermano.Datos[i]);
                }

                if (Hay_HIjos(hijo) == true)
                {
                    for (int i = 0; i < hermano.Nodos.Count; i++)
                    {
                        hijo.Nodos.Add(hermano.Nodos[i]);
                    }
                }

                padre.Nodos.RemoveAt(posicionvalorpadre + 1); //Elimina el nodo hermano
                hijo.Datos.Add(padre.Datos[0]); //Manda valor de raiz
                hijo.Datos.Sort((x, y) => x.CompareTo(y));

                padre.Datos.Remove(padre.Datos[0]);

                if (padre.Datos.Count < padre.Minimo && padre.Padre != null)
                {
                    VerificarHermanos(padre.Padre, padre);
                }
            }

            else if (hijo.Datos[0].CompareTo(padre.Datos[0]) > 0 && hijo.Datos[hijo.Datos.Count - 1].CompareTo(padre.Datos[padre.Datos.Count - 1]) < 0) //Si es uno de enmedio
            {
                if (hijo.Datos[0].CompareTo(padre.Datos[posicionvalorpadre]) > 0)
                {
                    hermano = padre.Nodos[posicionvalorpadre];
                    for (int i = 0; i < hermano.Datos.Count; i++)
                    {
                        hijo.Datos.Add(hermano.Datos[i]);
                    }

                    if (Hay_HIjos(hijo) == true)
                    {
                        for (int i = 0; i < hermano.Nodos.Count; i++)
                        {
                            hijo.Nodos.Add(hermano.Nodos[i]);
                        }
                    }

                    padre.Nodos.RemoveAt(posicionvalorpadre); //Elimina el nodo hermano
                    hijo.Datos.Add(padre.Datos[padre.Datos.Count - 1]); //Manda valor de raiz
                    hijo.Datos.Sort((x, y) => x.CompareTo(y));
                }
                else
                {
                    hermano = padre.Nodos[posicionvalorpadre + 1];
                    for (int i = 0; i < hermano.Datos.Count; i++)
                    {
                        hijo.Datos.Add(hermano.Datos[i]);
                    }

                    if (Hay_HIjos(hijo) == true)
                    {
                        for (int i = 0; i < hermano.Nodos.Count; i++)
                        {
                            hijo.Nodos.Add(hermano.Nodos[i]);
                        }
                    }

                    padre.Nodos.RemoveAt(posicionvalorpadre + 1); //Elimina el nodo hermano
                    hijo.Datos.Add(padre.Datos[posicionvalorpadre]); //Manda valor de raiz
                    padre.Datos.RemoveAt(posicionvalorpadre);
                    hijo.Datos.Sort((x, y) => x.CompareTo(y));
                }





            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IndiceValor"></param>
        /// <param name="Nodo"></param>
        /// <param name="IrDerecha"></param>
        /// <returns></returns>
        public NodoB MasIzquierdoDeDerecho(int IndiceValor, NodoB Nodo, bool IrDerecha)
        {
            if (IrDerecha == false && Nodo.Nodos.Count > 0)
            {
                IrDerecha = true;
                return MasIzquierdoDeDerecho(IndiceValor, Nodo.Nodos[IndiceValor + 1], IrDerecha);
            }
            else if (IrDerecha == true && Nodo.Nodos.Count > 0)
            {
                return MasIzquierdoDeDerecho(IndiceValor, Nodo.Nodos[0], IrDerecha);
            }
            else if (IrDerecha == true && Nodo.Nodos.Count == 0)
            {
                return Nodo;
            }
            else
            {
                return Nodo;
            }
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}