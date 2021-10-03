using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TesteGeneric
{
    class Program
    {
        async static Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await Test();

            Console.ReadKey();
        }

        public async static Task Test()
        {
            F facade = new F();

            var x = await facade.Get<Modelo>((x) => x.Command1);
            var y = await facade.Get<Modelo>((y) => y.Command2);

            var z = await facade.Get<Modelo, ModeloTextCommand>((z) => z.TextCommand1);
            var u = await facade.Get<Modelo, ModeloTextCommand>((u) => u.TextCommand2);
        }
    }

    public class F : IF
    {
        private readonly IB _b;

        public F()
        {
            _b = new B();
        }

        public async Task<IList<T>> Get<T>(Func<T, string> commantText) where T : class, new()
        {
            return await _b.Get<T>(commantText);
        }

        public async Task<IList<T>> Get<T, U>(Func<U, string> commandText)
            where T : class, new()
            where U : class, new()
        {
            return await _b.Get<T, U>(commandText);
        }
    }

    public interface IF : IB { }

    public class B : IB
    {
        private readonly IR _r;

        public B()
        {
            _r = new R();
        }

        public async Task<IList<T>> Get<T>(Func<T, string> commantText) where T : class, new()
        {
            return await _r.Get<T>(commantText((T)Activator.CreateInstance(typeof(T))));
        }

        public async Task<IList<T>> Get<T, U>(Func<U, string> commandText)
            where T : class, new()
                where U : class, new()
        {
            return await _r.Get<T>(commandText((U)Activator.CreateInstance(typeof(U))));
        }
    }

    public interface IB
    {
        Task<IList<T>> Get<T>(Func<T, string> commantText) where T : class, new();
        Task<IList<T>> Get<T, U>(Func<U, string> commandText)
            where T : class, new()
                where U : class, new();
    }

    public class R : IR
    {
        public async Task<IList<T>> Get<T>(string commandText) where T : class, new()
        {
            var x = await Task.Run(() => commandText);

            return new List<T>();
        }
    }

    public interface IR
    {
        Task<IList<T>> Get<T>(string commandText) where T : class, new();
    }

    public partial class Modelo
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
    }

    public partial class Modelo
    {
        public string Command1 => "modelo-text-command-01";
        public string Command2 => "modelo-text-command-02";
    }

    public class ModeloTextCommand
    {
        public string TextCommand1 => "text-command-01";
        public string TextCommand2 => "text-command-02";
    }
}
