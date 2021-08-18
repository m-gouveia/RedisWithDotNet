using System;
using StackExchange.Redis;

namespace ConsoleRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            var strCon = "localhost";
            var redis = ConnectionMultiplexer.Connect(strCon);
            IDatabase db = redis.GetDatabase();

            string equipe = "Grupo 1";
            string canal = "perguntas";

            var sub = redis.GetSubscriber();
            sub.Subscribe(canal, (ch, msg) =>
            {
                var arrMsg = msg.ToString().Split(":");

                string codPergunta = arrMsg[0].ToString();
                string corpoMsg = arrMsg[1].ToString();

                var nums = corpoMsg
                    .Replace(" ", "")
                    .Replace("Quantoeh", "")
                    .Replace("?", "")
                    .Split("+");

                var strNum1 = nums[0];
                var strNum2 = nums[1];

                int num1 = Convert.ToInt32(strNum1);
                int num2 = Convert.ToInt32(strNum2);

                int resposta = num1 + num2;

                db.HashSet(codPergunta, equipe, resposta);
            });

            Console.ReadLine();
        }
    }
}
