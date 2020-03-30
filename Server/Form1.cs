using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.IO;

namespace Server
{
    public partial class Form1 : Form
    {
        TcpListener server = null;// Ссылка на сервер
        int port = 12000;
        String hostName = "127.0.0.1";// local
        IPAddress localAddr;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            localAddr = IPAddress.Parse(hostName);// Конвертируем в другой формат

            Thread thread = new Thread(ExecuteLoop);
            thread.IsBackground = true;
            thread.Start();
        }
        public void Service1()
        {
          
        }

        private void ExecuteLoop()
        {
            try
            {
                server = new TcpListener(localAddr, port);// Создаем сервер-слушатель
                server.Start();// Запускаем сервер

                String data;

                // Бесконечный цикл прослушивания клиентов
                while (true)
                {
                    if (!server.Pending())// Очередь запросов пуста
                        continue;
                    TcpClient client = server.AcceptTcpClient();// Текущий клиент
                    // Сами задаем размеры буферов обмена (Необязательно!)
                    // По умолчанию оба буфера установлены размером по 8192 байта
                    client.SendBufferSize = client.ReceiveBufferSize = 1024;

                    // Подключаем NetworkStream и погружаем для удобства в оболочки
                    NetworkStream streamIn = client.GetStream();
                    NetworkStream streamOut = client.GetStream();
                    StreamReader readerStream = new StreamReader(streamIn);
                    StreamWriter writerStream = new StreamWriter(streamOut);

                    // Читаем запрос
                    data = readerStream.ReadLine();
                    textBox1.Text = data;
                    // Отправляем ответ
                    int index=0;

                    // if (int.TryParse(data.Substring(0, data.IndexOf('.')), out index))
                    if (data.Length > 0)
                    {
                        
                        index = 777777;
                    }
                    else
                        data = data.ToUpper();
                    writerStream.WriteLine(index);
                    writerStream.Flush();

                    // Закрываем соединение и потоки, порядок неважен
                    client.Close();
                    readerStream.Close();
                    writerStream.Close();
                }
            }
            catch (SocketException)
            {
            }
            finally
            {
                // Останавливаем сервер
                server.Stop();
            }
        }
    }
}

