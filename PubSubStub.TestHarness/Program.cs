using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PubSubStub.Collections.Generic;
using PubSubStub.TestHarness.Model;

namespace PubSubStub.TestHarness
{
    public class Program
    {
        const string OutputFormat = "Subscriber: {0} - {1} / {2}";
        private const int max = 50;
        private static Publisher<Model1> _model1Publisher = new Publisher<Model1>();
        private static Publisher<Model2> _model2Publisher = new Publisher<Model2>();
        

        private static readonly ConcurrentCollection<Subscriber<Model1>> _model1Subscribers = new ConcurrentCollection<Subscriber<Model1>>();
        private static readonly ConcurrentCollection<Subscriber<Model2>> _model2Subscribers = new ConcurrentCollection<Subscriber<Model2>>();

        public static void Main(string[] args)
        {
            _model1Publisher = new Publisher<Model1>();
            _model2Publisher = new Publisher<Model2>();
            PublisherFactory.Instance.Register(_model1Publisher);
            PublisherFactory.Instance.Register(_model2Publisher);

            // subscribers
            for (var i = 0; i < max; i++)
            {
                SetModel1(i);
                SetModel2(i);
            }

            new Thread(Publish).Start();
            new Thread(Update).Start();
        }

        private static void Update()
        {
            while (true)
            {
                var rnd = new Random().Next(1, max * 2);
                SetModel1(rnd - 1);
                SetModel2(rnd - 1);
                Thread.Sleep((int) Math.Floor((float)rnd/10));
            }
        }

        private static void Publish()
        {
            while (true)
            {
                var rnd = new Random().Next(1, max * 2);
                _model1Publisher.Publish(new Model1 { Description = "DESC: " + rnd, Name = "NAME: " + rnd });
                _model2Publisher.Publish(new Model2 { Message = "MSG: " + rnd, Name = "NAME: " + rnd });

                Thread.Sleep((int)Math.Floor((float)rnd / 10));
            }
        }

        private static void SetModel1(int index)
        {
            var sub = _model1Subscribers.ElementAtOrDefault(index);
            if (sub != null)
            {
                sub.Dispose();
                Console.WriteLine("Disposed Model1: " + index);
            }

            sub = new Subscriber<Model1>();
            sub.BindOnNext((data) => Console.WriteLine(OutputFormat, "Model1 " + index, data.Name, data.Description));
            sub.Subscribe(_model1Publisher);
            _model1Subscribers.Add(sub);
        }

        private static void SetModel2(int index)
        {
            var sub = _model2Subscribers.ElementAtOrDefault(index);
            if (sub != null)
            {
                sub.Dispose();
                Console.WriteLine("Disposed Model2: " + index);
            }

            sub = new Subscriber<Model2>();
            sub.BindOnNext((data) => Console.WriteLine(OutputFormat, "Model2 " + index, data.Name, data.Message));
            sub.Subscribe(_model2Publisher);
            _model2Subscribers.Add(sub);
        }
    }
}
