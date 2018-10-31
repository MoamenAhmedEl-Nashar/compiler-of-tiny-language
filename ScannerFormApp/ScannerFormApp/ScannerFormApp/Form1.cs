using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScannerFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public List<string> values=new List<string>();
        public List<string> types=new List<string>();

        private void Start_Click(object sender, EventArgs e)
        {
            string code = input.Text.ToString()+" ";//space added at last of code to simulate [other]
            scanner(code);
        }

        private bool isSymbol(char str)
        {
            if (str == '+' || str == '-' ||
               str == '*' || str == '/' ||
               str == '=' || str == '<' ||
               str == '(' || str == ')' ||
               str == ';' )
                return true;
            else return false;
        }

        private bool isReserved(string str)
        {
            if (str == "if" || str == "then" || str == "else" ||
               str == "end" || str == "repeat" || str == "until" ||
               str == "read" || str == "write")
                return true;
            else return false;
        }

        
        private void scanner(string code)
        {
            int state = 0;
            string current_string = "";
            var builder = new StringBuilder();
            int i = 0;
            while (i < code.Length)
            {
                switch (state)
                {
                    case 0://start
                        if(code[i]==' ' || code[i]=='\n')
                        {
                            state = 0;
                            i++;
                        }
                        else if(code[i]=='{')
                        {
                            state = 1;
                            i++;
                        }
                        else if(isSymbol(code[i]))
                        {
                            builder.Append(code[i]);
                            current_string = builder.ToString();
                            values.Add(current_string);
                            types.Add("symbol");
                            current_string = "";
                            builder.Clear();
                            state = 0;
                            i++;
                        }
                        else if(char.IsDigit(code[i]))
                        {
                            builder.Append(code[i]);
                            current_string = builder.ToString();
                            state = 2;//number
                            i++;
                        }
                        else if (char.IsLetter(code[i]))
                        {
                            builder.Append(code[i]);
                            current_string = builder.ToString();
                            state = 3;//id
                            i++;
                        }
                        else if(code[i]==':')
                        {
                            builder.Append(code[i]);
                            current_string = builder.ToString();
                            state = 4;//assign
                            i++;
                        }
                        break;

                    case 1://comment
                        if(code[i]!='}')
                        {
                            state = 1;//comment
                            i++;
                        }
                        else
                        {
                            state = 0;//start
                            i++;
                        }
                        break;
                    case 2://number
                        if (char.IsDigit(code[i]))
                        {
                            builder.Append(code[i]);
                            current_string = builder.ToString();
                            state = 2;
                            i++;
                        }
                        else
                        {
                            values.Add(current_string);
                            types.Add("number");
                            current_string = "";
                            builder.Clear();
                            state = 0;
                        }
                        break;
                    case 3://id
                        if (char.IsLetterOrDigit(code[i]))
                        {
                            builder.Append(code[i]);
                            current_string = builder.ToString();
                            state = 3;
                            i++;
                        }
                        else
                        {
                            values.Add(current_string);
                            if(isReserved(current_string))
                            {
                                types.Add("reserved");
                            }
                            else
                            {
                                types.Add("identifier");
                            }
                            current_string = "";
                            builder.Clear();
                            state = 0;
                        }
                        break;
                    case 4://assign
                        if (code[i]=='=')
                        {
                            builder.Append(code[i]);
                            current_string = builder.ToString();
                            values.Add(current_string);
                            types.Add("symbol");
                            current_string = "";
                            builder.Clear();
                            state = 0;
                            i++;
                        }
                        

                        break;
                }
            }
        }



    }
}
