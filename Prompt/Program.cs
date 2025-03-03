using System;
using System.Collections.Generic;
public class Procesamiento
{
    static async Task Main()
    {
        List<string> imagenes = new List<string>() {"img1","img2","img3","img4","img5","img6","img7","img8","img9","img10"};

        foreach(var image in imagenes)
        {
            await imagenProcesada(image);
        }
    }

    public static async Task imagenProcesada(string image)
    {
        Console.WriteLine($"Procesando la imagen{image}");
        await Task.Delay(2000);
        var cts = new System.Threading.CancellationToken();

        var Filtrando = Task.Factory.StartNew(async()=> 
        {
            
            List<Task> Filtros = new List<Task> ()
            {
                Task.Run(async ()=>
                {
                    await AplicarFiltro(image, "Mejora de Contraste", cts);
                    await AplicarFiltro(image, "Escala de Grises", cts);
                    await AplicarFiltro(image, "Desenfoque", cts);
                })
            };

            while (Filtros.Count > 0)
            {

                Task filtroTerminado = await Task.WhenAny(Filtros);
                Filtros.Remove(filtroTerminado);
                Console.WriteLine($"Filtro aplicado en {image}");
            }
        }, TaskCreationOptions.AttachedToParent);

        Filtrando.ContinueWith(imgprocesada =>
        {
            Console.WriteLine($"La {imgprocesada} fue completada exitosamente!!!");
        }, TaskContinuationOptions.OnlyOnRanToCompletion);

        Filtrando.ContinueWith(imgprocesada =>
        {
            Console.WriteLine($"La {imgprocesada} no logro completarse exitosamente");
        }, TaskContinuationOptions.OnlyOnCanceled);

        await Filtrando;
    }

    public static async Task AplicarFiltro(string image, string nombre, System.Threading.CancellationToken token)
    {
        Console.WriteLine($"Aplicando filtro {nombre} a la {image}");
        await Task.Delay(new Random().Next(100, 3000), token);
        Console.WriteLine($"Filtro aplicado {nombre} a la {image}");

    }
}