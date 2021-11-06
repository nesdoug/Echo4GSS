using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Echo4GSS
{
    public partial class Form1 : Form
    {
        

        public static int main_vol = 0x70;
        public static int echo_start = 0xdf;
        public static int echo_size = 4; // aka delay
        public static int echo_vol = 0x30;
        public static int echo_fb = 0x50;
        public static int fir_c0 = 0x7f;
        public static int fir_c1 = 0;
        public static int fir_c2 = 0;
        public static int fir_c3 = 0;
        public static int fir_c4 = 0;
        public static int fir_c5 = 0;
        public static int fir_c6 = 0;
        public static int fir_c7 = 0;
        

        public static byte[] SPC_Array = new byte[0x10200];
        public static byte[] Small_Array = new byte[14];

        public static bool has_loaded = false;
        public static int spc_size;
        public static bool safe_to_save = true;

        public static string name_f = "";
        public static string path_f = "";

        public static int number_of_brrs = 0;
        public static int adsr_address = 0;
        public static int adsr_address2 = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox8.Checked = true;

        }

        private bool is_hex(char ch1)
        {
            if ((ch1 >= '0') && (ch1 <= '9')) return true;
            if ((ch1 >= 'A') && (ch1 <= 'F')) return true;
            //should be upper case letters
            return false;
        }

        public int Hex_Str_to_Int(string str)
        {
            // assumed str != ""
            int out_val = 0;
            if (is_hex(str[0]) == false) str = "0";
            else if ((str.Length > 1) && ((is_hex(str[1]) == false)))
            {
                str = "0";
            }

            out_val = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
            
            return out_val;
        }
        


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;
            tb1_set();
        }

        public void tb1_set()
        {
            // Echo Start Address
            string str = textBox1.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            echo_start = value & 0xff;
            str = value.ToString("X");
            textBox1.Text = str;
            textBox1.SelectionStart = textBox1.Text.Length;

            Error_Check();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "") return;
            tb2_set();
        }

        public void tb2_set()
        {
            // Echo Size
            string str = textBox2.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            value = value & 0x0f; // one digit only
            echo_size = value;
            str = value.ToString("X");
            textBox2.Text = str;
            textBox2.SelectionStart = textBox2.Text.Length;

            Error_Check();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "") return;
            tb3_set();
        }

        public void tb3_set()
        {
            // Echo Volume
            string str = textBox3.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            //echo_vol = value & 0xff;
            if (value > 0xff) value = 0xff;
            if (value < 0) value = 0;
            echo_vol = value;
            str = value.ToString("X");
            textBox3.Text = str;
            textBox3.SelectionStart = textBox3.Text.Length;

            Error_Check();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "") return;
            tb4_set();
        }

        public void tb4_set()
        {
            // Echo Feedback
            string str = textBox4.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            if (value > 0xff) value = 0xff;
            if (value < 0) value = 0;
            echo_fb = value;
            str = value.ToString("X");
            textBox4.Text = str;
            textBox4.SelectionStart = textBox4.Text.Length;

            Error_Check();

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "") return;
            tb5_set();
        }

        public void tb5_set()
        {
            // FIR c0
            string str = textBox5.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c0 = value & 0xff;
            str = value.ToString("X");
            textBox5.Text = str;
            textBox5.SelectionStart = textBox5.Text.Length;

            Error_Check();

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "") return;
            tb6_set();
        }

        public void tb6_set()
        {
            // FIR c1
            string str = textBox6.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c1 = value & 0xff;
            str = value.ToString("X");
            textBox6.Text = str;
            textBox6.SelectionStart = textBox6.Text.Length;

            Error_Check();

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "") return;
            tb7_set();
        }

        public void tb7_set()
        {
            // FIR c2
            string str = textBox7.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c2 = value & 0xff;
            str = value.ToString("X");
            textBox7.Text = str;
            textBox7.SelectionStart = textBox7.Text.Length;

            Error_Check();

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text == "") return;
            tb8_set();
        }

        public void tb8_set()
        {
            // FIR c3
            string str = textBox8.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c3 = value & 0xff;
            str = value.ToString("X");
            textBox8.Text = str;
            textBox8.SelectionStart = textBox8.Text.Length;

            Error_Check();

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text == "") return;
            tb9_set();
        }

        public void tb9_set()
        {
            // FIR c4
            string str = textBox9.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c4 = value & 0xff;
            str = value.ToString("X");
            textBox9.Text = str;
            textBox9.SelectionStart = textBox9.Text.Length;

            Error_Check();

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text == "") return;
            tb10_set();
        }

        public void tb10_set()
        {
            // FIR c5
            string str = textBox10.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c5 = value & 0xff;
            str = value.ToString("X");
            textBox10.Text = str;
            textBox10.SelectionStart = textBox10.Text.Length;

            Error_Check();

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text == "") return;
            tb11_set();
        }

        private void tb11_set()
        {
            // FIR c6
            string str = textBox11.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c6 = value & 0xff;
            str = value.ToString("X");
            textBox11.Text = str;
            textBox11.SelectionStart = textBox11.Text.Length;

            Error_Check();

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text == "") return;
            tb12_set();
        }

        public void tb12_set()
        {
            // FIR c7
            string str = textBox12.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            fir_c7 = value & 0xff;
            str = value.ToString("X");
            textBox12.Text = str;
            textBox12.SelectionStart = textBox12.Text.Length;

            Error_Check();

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "0";
            }
            tb1_set();
            // reset to normal
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = textBox1.Text.Length;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "0";
            }
            tb2_set();
            // reset to normal
            textBox2.SelectionStart = 0;
            textBox2.SelectionLength = textBox2.Text.Length;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "0";
            }
            tb3_set();
            // reset to normal
            textBox3.SelectionStart = 0;
            textBox3.SelectionLength = textBox3.Text.Length;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = "0";
            }
            tb4_set();
            // reset to normal
            textBox4.SelectionStart = 0;
            textBox4.SelectionLength = textBox4.Text.Length;
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.Text = "0";
            }
            tb5_set();
            // reset to normal
            textBox5.SelectionStart = 0;
            textBox5.SelectionLength = textBox5.Text.Length;
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.Text = "0";
            }
            tb6_set();
            // reset to normal
            textBox6.SelectionStart = 0;
            textBox6.SelectionLength = textBox6.Text.Length;
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                textBox7.Text = "0";
            }
            tb7_set();
            // reset to normal
            textBox7.SelectionStart = 0;
            textBox7.SelectionLength = textBox7.Text.Length;
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                textBox8.Text = "0";
            }
            tb8_set();
            // reset to normal
            textBox8.SelectionStart = 0;
            textBox8.SelectionLength = textBox8.Text.Length;
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            if (textBox9.Text == "")
            {
                textBox9.Text = "0";
            }
            tb9_set();
            // reset to normal
            textBox9.SelectionStart = 0;
            textBox9.SelectionLength = textBox9.Text.Length;
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            if (textBox10.Text == "")
            {
                textBox10.Text = "0";
            }
            tb10_set();
            // reset to normal
            textBox10.SelectionStart = 0;
            textBox10.SelectionLength = textBox10.Text.Length;
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            if (textBox11.Text == "")
            {
                textBox11.Text = "0";
            }
            tb11_set();
            // reset to normal
            textBox11.SelectionStart = 0;
            textBox11.SelectionLength = textBox11.Text.Length;
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            if (textBox12.Text == "")
            {
                textBox12.Text = "0";
            }
            tb12_set();
            // reset to normal
            textBox12.SelectionStart = 0;
            textBox12.SelectionLength = textBox12.Text.Length;
        }


        


        private void openSPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open spc file

            // 0x10200 bytes exactly, starts with...
            // 53 4E 45 53 2D 53 50 43 37 30 30

            // SPC_Array[]

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Open an SPC File";
            openFileDialog1.Filter = "SPC File (*.spc)|*.spc|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = (System.IO.FileStream)openFileDialog1.OpenFile();

                if (fs.Length == 0x10200)
                {
                    for (int i = 0; i < 0x10200; i++)
                    {
                        SPC_Array[i] = (byte)fs.ReadByte();
                    }

                    if ((SPC_Array[0] == 0x53) &&
                        (SPC_Array[1] == 0x4e) &&
                        (SPC_Array[2] == 0x45) &&
                        (SPC_Array[3] == 0x53) &&
                        (SPC_Array[4] == 0x2d) &&
                        (SPC_Array[5] == 0x53) &&
                        (SPC_Array[6] == 0x50) &&
                        (SPC_Array[7] == 0x43) &&
                        (SPC_Array[8] == 0x37) &&
                        (SPC_Array[9] == 0x30) &&
                        (SPC_Array[10] == 0x30) )
                    {
                        has_loaded = true;

                        // print how many bytes the SPC uses
                        int size = 0x100ff;
                        for(  ;size >= 0x100; size--)
                        {
                            if (SPC_Array[size] != 0) break;
                        }
                        size -= 0xff; 
                        label10.Text = size.ToString("X");
                        spc_size = size; // = first safe byte


                        Error_Check();

                        string name = "Echo For SNESGSS  [";
                        name_f = Path.GetFileNameWithoutExtension(fs.Name);
                        name += name_f;
                        name += "]";
                        this.Text = name;
                        path_f = Path.GetDirectoryName(fs.Name);


                        //number_of_brrs
                        // pointer to ADSR at SPC 208
                        adsr_address = SPC_Array[0x308] + (SPC_Array[0x309] << 8);
                        adsr_address += 18;
                        adsr_address2 = adsr_address + 0x100;

                        number_of_brrs = (adsr_address - 0xb24) / 4;
                        label13.Text = number_of_brrs.ToString();
                    }
                    else
                    {
                        MessageBox.Show("File error. Not an SPC ?");
                    }

                }
                else
                {
                    MessageBox.Show("File size error. Expected 0x10200 bytes.");
                }
                fs.Close();

            }

        }


        public void Error_Check()
        {
            bool error1 = false;
            bool error2 = false;

            int actual_echo_start = echo_start * 0x100;
            int actual_echo_size = echo_size * 0x800;
            if(echo_size == 0)
            {
                actual_echo_size = 32;
            }

            safe_to_save = true;

            if (has_loaded == false) return;

            if (spc_size > actual_echo_start)
            {
                error1 = true;
                safe_to_save = false;
            }
            if((actual_echo_start + actual_echo_size) > 0xffc0)
            {
                error2 = true;
                safe_to_save = false;
            }
            
            if((error1 == false) && (error2 == false))
            {
                // both false
                label8.Text = "Seems OK.";
                return;
            }

            string temp_str = "";
            if(error1 == true)
            {
                temp_str += "Start too Low. ";
            }
            if(error2 == true)
            {
                temp_str += "Size overflows. ";
            }
            label8.Text = temp_str;
        }


        private void saveSPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save spc file
            if(has_loaded == false)
            {
                MessageBox.Show("Error. SPC hasn't been loaded yet.");
                return;
            }

            if(safe_to_save == false)
            {
                MessageBox.Show("Error. Fix size errors before saving.");
                return;
            }

            // modify the ROM
            SPC_Array[0xBA8] = (byte)echo_start;
            SPC_Array[0xBAA] = (byte)echo_size; // aka delay
            SPC_Array[0xBAC] = (byte)echo_fb;
            SPC_Array[0xBAE] = (byte)fir_c0;
            SPC_Array[0xBB0] = (byte)fir_c1;
            SPC_Array[0xBB2] = (byte)fir_c2;
            SPC_Array[0xBB4] = (byte)fir_c3;
            SPC_Array[0xBB6] = (byte)fir_c4;
            SPC_Array[0xBB8] = (byte)fir_c5;
            SPC_Array[0xBBA] = (byte)fir_c6;
            SPC_Array[0xBBC] = (byte)fir_c7;
            int temp1 = Shift_Checkboxes();
            SPC_Array[0xBBE] = (byte)temp1;
            // NOTE we aren't changing echo volume yet
            SPC_Array[0xBC8] = 0; // enable echo, flag

            // main vol
            temp1 = (main_vol & 0x7f) * 2;
            SPC_Array[0x7CC] = (byte)temp1;

            // overwriting streamClearBuffers
            // with a wait loop
            SPC_Array[0x4E8] = 0xcd;
            SPC_Array[0x4E9] = 0; // ldx #0
            SPC_Array[0x4EA] = 0x8d;
            SPC_Array[0x4EB] = 0; // ldy #0
            SPC_Array[0x4EC] = 0; // nop
            SPC_Array[0x4ED] = 0; // nop
            SPC_Array[0x4EE] = 0x1d; // dex
            SPC_Array[0x4EF] = 0xd0;
            SPC_Array[0x4F0] = 0xfc; // bne -4
            SPC_Array[0x4F1] = 0xfe; // dey and
            SPC_Array[0x4F2] = 0xfa; // bne -6
            // Finally, turn on echo volume
            SPC_Array[0x4F3] = 0xcd;
            SPC_Array[0x4F4] = 0x2c; // ldx #2c
            SPC_Array[0x4F5] = 0xd8;
            SPC_Array[0x4F6] = 0xf2; // stx f2
            SPC_Array[0x4F7] = 0xe8;
            SPC_Array[0x4F8] = (byte)echo_vol; // lda vol
            SPC_Array[0x4F9] = 0xc4;
            SPC_Array[0x4FA] = 0xf3; // sta f3
            SPC_Array[0x4FB] = 0xcd;
            SPC_Array[0x4FC] = 0x3c; // ldx #3c
            SPC_Array[0x4FD] = 0xd8;
            SPC_Array[0x4FE] = 0xf2; // stx f2
            SPC_Array[0x4FF] = 0xc4;
            SPC_Array[0x500] = 0xf3; // sta f3
            SPC_Array[0x501] = 0x6f; // rts

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "SPC File (*.spc)|*.spc|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save SPC File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                for (int i = 0; i < 0x10200; i++)
                {
                    fs.WriteByte((byte)SPC_Array[i]);
                }
                fs.Close();

                MessageBox.Show("Success!");
            }

        }

        private void loadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // load settings, 14 bytes
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Open Settings";
            openFileDialog1.Filter = "BIN File (*.bin)|*.bin|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                System.IO.FileStream fs = (System.IO.FileStream)openFileDialog1.OpenFile();

                if (fs.Length == 14)
                {
                    for (int i = 0; i < 14; i++)
                    {
                        Small_Array[i] = (byte)fs.ReadByte();
                    }

                    // rebuild the values and boxes
                    Rebuild_Checkboxes();
                    echo_start = Small_Array[1];
                    echo_size = Small_Array[2];
                    echo_vol = Small_Array[3];
                    echo_fb = Small_Array[4];
                    fir_c0 = Small_Array[5];
                    fir_c1 = Small_Array[6];
                    fir_c2 = Small_Array[7];
                    fir_c3 = Small_Array[8];
                    fir_c4 = Small_Array[9];
                    fir_c5 = Small_Array[10];
                    fir_c6 = Small_Array[11];
                    fir_c7 = Small_Array[12];
                    main_vol = Small_Array[13];
                    Rebuild_Boxes();
                    Error_Check();
                }
                else
                {
                    MessageBox.Show("File size error. Expected 14 bytes.");
                }

                fs.Close();
                
            }
        }

        public int Shift_Checkboxes()
        {
            int value = 0;
            if (checkBox1.Checked == true) value += 1;
            if (checkBox2.Checked == true) value += 2;
            if (checkBox3.Checked == true) value += 4;
            if (checkBox4.Checked == true) value += 8;
            if (checkBox5.Checked == true) value += 16;
            if (checkBox6.Checked == true) value += 32;
            if (checkBox7.Checked == true) value += 64;
            if (checkBox8.Checked == true) value += 128;

            return value;
        }


        public void Rebuild_Checkboxes()
        {
            if ((Small_Array[0] & 0x01) != 0) checkBox1.Checked = true;
            else checkBox1.Checked = false;

            if ((Small_Array[0] & 0x02) != 0) checkBox2.Checked = true;
            else checkBox2.Checked = false;

            if ((Small_Array[0] & 0x04) != 0) checkBox3.Checked = true;
            else checkBox3.Checked = false;

            if ((Small_Array[0] & 0x08) != 0) checkBox4.Checked = true;
            else checkBox4.Checked = false;

            if ((Small_Array[0] & 0x10) != 0) checkBox5.Checked = true;
            else checkBox5.Checked = false;

            if ((Small_Array[0] & 0x20) != 0) checkBox6.Checked = true;
            else checkBox6.Checked = false;

            if ((Small_Array[0] & 0x40) != 0) checkBox7.Checked = true;
            else checkBox7.Checked = false;

            if ((Small_Array[0] & 0x80) != 0) checkBox8.Checked = true;
            else checkBox8.Checked = false;
        }

        public void Rebuild_Boxes()
        {
            int value;

            value = echo_start & 0xff;
            textBox1.Text = value.ToString("X");
            value = echo_size & 0x0f; // only 1 digit
            textBox2.Text = value.ToString("X");
            value = echo_vol & 0xff;
            textBox3.Text = value.ToString("X");
            value = echo_fb & 0xff;
            textBox4.Text = value.ToString("X");

            value = fir_c0 & 0xff;
            textBox5.Text = value.ToString("X");
            value = fir_c1 & 0xff;
            textBox6.Text = value.ToString("X");
            value = fir_c2 & 0xff;
            textBox7.Text = value.ToString("X");
            value = fir_c3 & 0xff;
            textBox8.Text = value.ToString("X");
            value = fir_c4 & 0xff;
            textBox9.Text = value.ToString("X");
            value = fir_c5 & 0xff;
            textBox10.Text = value.ToString("X");
            value = fir_c6 & 0xff;
            textBox11.Text = value.ToString("X");
            value = fir_c7 & 0xff;
            textBox12.Text = value.ToString("X");
            value = main_vol & 0xff;
            textBox13.Text = value.ToString("X");

        }


        public void Rebuild_Sm_Array()
        {
            Small_Array[0] = (byte)Shift_Checkboxes();
            Small_Array[1] = (byte)echo_start;
            Small_Array[2] = (byte)echo_size;
            Small_Array[3] = (byte)echo_vol;
            Small_Array[4] = (byte)echo_fb;
            Small_Array[5] = (byte)fir_c0;
            Small_Array[6] = (byte)fir_c1;
            Small_Array[7] = (byte)fir_c2;
            Small_Array[8] = (byte)fir_c3;
            Small_Array[9] = (byte)fir_c4;
            Small_Array[10] = (byte)fir_c5;
            Small_Array[11] = (byte)fir_c6;
            Small_Array[12] = (byte)fir_c7;
            Small_Array[13] = (byte)main_vol;
        }


        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save settings, 14 bytes

            // generate the array
            Rebuild_Sm_Array();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "BIN File (*.bin)|*.bin|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save Settings";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                for (int i = 0; i < 14; i++)
                {
                    fs.WriteByte((byte)Small_Array[i]);
                }
                fs.Close();
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.Text == "") return;
            tb13_set();
        }

        public void tb13_set()
        {
            // MAIN VOL
            string str = textBox13.Text.ToUpper(); // upper case
            int value = Hex_Str_to_Int(str);
            if (value > 0x7f) value = 0x7f;
            if (value < 0) value = 0;
            main_vol = value;
            str = value.ToString("X");
            textBox13.Text = str;
            textBox13.SelectionStart = textBox13.Text.Length;

            Error_Check();

        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            if (textBox13.Text == "")
            {
                textBox13.Text = "0";
            }
            tb13_set();
            // reset to normal
            textBox13.SelectionStart = 0;
            textBox13.SelectionLength = textBox13.Text.Length;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            textBox1.Text = echo_start.ToString("X");
            textBox2.Text = echo_size.ToString("X");
            textBox3.Text = echo_vol.ToString("X");
            textBox4.Text = echo_fb.ToString("X");
            textBox5.Text = fir_c0.ToString("X");
            textBox6.Text = fir_c1.ToString("X");
            textBox7.Text = fir_c2.ToString("X");
            textBox8.Text = fir_c3.ToString("X");
            textBox9.Text = fir_c4.ToString("X");
            textBox10.Text = fir_c5.ToString("X");
            textBox11.Text = fir_c6.ToString("X");
            textBox12.Text = fir_c7.ToString("X");
            textBox13.Text = main_vol.ToString("X");
        }

        private void extract1BRRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save 1 brr file
            if (has_loaded == false)
            {
                MessageBox.Show("Error. SPC hasn't been loaded yet.");
                return;
            }
            if(comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Error? Try Selecting a BRR again.");
                return;
            }
            if(comboBox1.SelectedIndex >= number_of_brrs)
            {
                MessageBox.Show("Error. BRR selection too high.");
                return;
            }

            int offset = comboBox1.SelectedIndex;
            offset *= 4;
            offset += 0xc24;

            int brr_start = SPC_Array[offset] + (SPC_Array[offset + 1] << 8);
            int brr_loop = SPC_Array[offset + 2] + (SPC_Array[offset + 3] << 8);

            brr_start += 0x100; // spc file has 0x100 bytes before the actual file
            brr_loop += 0x100;

            int brr_end = -1;
            // check all header bytes until you find an end bit
            for(int i = brr_start; i < 0x10100; i += 9)
            {
                if ((SPC_Array[i] & 1) == 1) // 1 = end
                {
                    brr_end = i + 9;
                    break;
                }
            }
            if (brr_end >= 0x10100) brr_end = -1;

            if (brr_end == -1)
            {
                MessageBox.Show("Error. Couldn't find end of BRR");
                return;
            }
            int brr_size = brr_end - brr_start;
            
            if ((brr_size % 9) != 0)
            {
                MessageBox.Show("Error. BRR Not Divisible by 9");
                return;
            }
            if (brr_size > 0xf400)
            {
                MessageBox.Show("Error. BRR Was Too Large.");
                return;
            }
            if((brr_loop < brr_start) ||
               (brr_loop >= brr_end))
            {
                MessageBox.Show("Error. Loop point outside of BRR");
                return;
            }

            int loop_offset = brr_loop - brr_start;

            if((SPC_Array[brr_start] & 2) != 2) // 2 = looped
            {
                loop_offset = 0;
            }
            

            // now save a binary file, the BRR

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "BRR File (*.brr)|*.brr|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save a BRR File";
            int name_index = comboBox1.SelectedIndex + 1;
            string str = name_f;
            str += "_";
            if(name_index < 10)
            {
                str += "0";
            }
            str += name_index.ToString();
            saveFileDialog1.FileName = str; // auto generate a name
            saveFileDialog1.ShowDialog();


            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                if(checkBox9.Checked == true)
                {
                    fs.WriteByte((byte)loop_offset);
                    fs.WriteByte((byte)(loop_offset >> 8));
                }

                for (int i = brr_start; i < brr_end; i++)
                {
                    fs.WriteByte((byte)SPC_Array[i]);
                }
                fs.Close();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            // if you type into the combobox
            // make sure it actually selects an item
            comboBox1.SelectedIndex = comboBox1.FindStringExact(comboBox1.Text);
            comboBox1.SelectionStart = comboBox1.Text.Length;
        }

        private void saveInfoAsTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // loop through all 99 brr files and
            // write a text file with their
            // start position, loop position, end, ? loop

            if (has_loaded == false)
            {
                MessageBox.Show("Error. SPC hasn't been loaded yet.");
                return;
            }

            string str = "";
            string big_str = "Info on the BRR files in ";
            big_str += name_f.ToString();
            big_str += "\r\n\r\n";

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                string str2 = name_f;
                str2 += "_Info.txt";
                sfd.FileName = str2; // auto generate a name

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    for(int i = 0; i < number_of_brrs; i++)
                    {
                        str = GetInfo(i);
                        big_str += str;
                    }
                    
                    big_str += "SPC Size: $";
                    big_str += spc_size.ToString("X");
                    big_str += "\r\n\r\n";

                    File.WriteAllText(sfd.FileName, big_str);

                }
            }
            
        }

        public string GetInfo(int index)
        {
            // scan each brr and return a string about it's info
            // start position, loop position, end, ? loop
            // skip all the error checking junk

            int index2 = index + 1;
            string str = index2.ToString();
            str += "  ";

            int offset = index;
            offset *= 4;
            offset += 0xc24;
            int brr_start = SPC_Array[offset] + (SPC_Array[offset + 1] << 8);
            int brr_start2 = brr_start + 0x100;
            int brr_loop = SPC_Array[offset + 2] + (SPC_Array[offset + 3] << 8);
            int brr_loop2 = brr_loop + 0x100;
            int brr_end = -1;
            // check all header bytes until you find an end bit
            for (int i = brr_start2; i < 0x10100; i += 9)
            {
                if ((SPC_Array[i] & 1) == 1) // 1 = end
                {
                    brr_end = i + 9;
                    break;
                }
            }
            if (brr_end >= 0x10100) brr_end = 0;
            if (brr_end < 0) brr_end = 0;

            if(brr_end == 0)
            {
                str += "Error 1\r\n";
                return str;
            }
            int brr_size = brr_end - brr_start2;
            if((brr_size % 9) != 0)
            {
                str += "Error 2\r\n";
                return str;
            }
            if((brr_size < 9) || (brr_size > 0xf400))
            {
                str += "Error 3\r\n";
                return str;
            }
            if ((brr_loop < brr_start) ||
               (brr_loop >= brr_end))
            {
                str += "Error 4\r\n";
                return str;
            }

            int loop_offset = brr_loop - brr_start;
            bool we_are_looping = true;
            if ((SPC_Array[brr_start2] & 2) != 2) // 2 = looped
            {
                loop_offset = 0;
                we_are_looping = false;
            }

            // print everything now
            str += "Addr: $";
            str += brr_start.ToString("X");
            str += "  Size: $";
            str += brr_size.ToString("X");
            str += "  Loop: $";
            str += loop_offset.ToString("X");
            
            if(we_are_looping == true)
            {
                str += "  looped";
            }
            else
            {
                str += "  no loop";
            }
            str += "\r\n";

            // adsr
            str += "ADSR: ";
            int adsr1 = SPC_Array[adsr_address2 + (index * 2)];
            str += adsr1.ToString("X");
            str += " ";
            int adsr2 = SPC_Array[adsr_address2 + (index * 2) + 1];
            str += adsr2.ToString("X");
            str += " = ";
            // decode the adsr values
            int attack = adsr1 & 15;
            int delay = (adsr1 >> 4) & 7;
            int sustain = (adsr2 >> 5) & 7;
            int release = adsr2 & 31;
            str += attack.ToString();
            str += " ";
            str += delay.ToString();
            str += " ";
            str += sustain.ToString();
            str += " ";
            str += release.ToString();
            str += "\r\n";
            str += "\r\n";

            return str;
        }



        private void extractAllBRRsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //path_f = where to generate the BRR files
            //name_f = name of SPC file

            int count_brrs = 0;

            if (has_loaded == false)
            {
                MessageBox.Show("Error. SPC hasn't been loaded yet.");
                return;
            }
            if ((path_f == "") || (name_f == ""))
            {
                MessageBox.Show("Error. Blank path or filename.");
                return;
            }

            for (int index = 0; index < number_of_brrs; index++)
            {
                string part2 = name_f + "_";
                int j = index + 1;
                if(j < 10)
                {
                    part2 += "0";
                }
                part2 += j.ToString();
                part2 += ".brr";


                int offset = index;
                offset *= 4;
                offset += 0xc24;
                int brr_start = SPC_Array[offset] + (SPC_Array[offset + 1] << 8);
                int brr_start2 = brr_start + 0x100;
                int brr_loop = SPC_Array[offset + 2] + (SPC_Array[offset + 3] << 8);
                int brr_loop2 = brr_loop + 0x100;
                int brr_end = -1;
                // check all header bytes until you find an end bit
                for (int i = brr_start2; i < 0x10100; i += 9)
                {
                    if ((SPC_Array[i] & 1) == 1) // 1 = end
                    {
                        brr_end = i + 9;
                        break;
                    }
                }
                if (brr_end >= 0x10100) brr_end = 0;
                if (brr_end < 0) brr_end = 0;

                if (brr_end == 0)
                {
                    continue;
                }
                int brr_size = brr_end - brr_start2;
                if ((brr_size % 9) != 0)
                {
                    continue;
                }
                if ((brr_size < 9) || (brr_size > 0xf400))
                {
                    continue;
                }
                if ((brr_loop < brr_start) ||
                   (brr_loop >= brr_end))
                {
                    continue;
                }

                int loop_offset = brr_loop - brr_start;
                
                if ((SPC_Array[brr_start2] & 2) != 2) // 2 = looped
                {
                    loop_offset = 0;
                    
                }

                string slash = Path.DirectorySeparatorChar.ToString();
                string full_path = path_f + slash + part2;
                
                //now save a new binary BRR file to same directory as SPC
                using (FileStream fs = File.Create(@full_path))
                {
                    if (checkBox9.Checked == true)
                    {
                        fs.WriteByte((byte)loop_offset);
                        fs.WriteByte((byte)(loop_offset >> 8));
                    }
                    fs.Write(SPC_Array, brr_start2, brr_size);
                }
                count_brrs++;
            }

            if(count_brrs == 0)
            {
                MessageBox.Show("Error. Zero BRRs found.");
            }
            else
            {
                string message = count_brrs.ToString();
                message += " BRR files created in ";
                message += path_f;
                MessageBox.Show(message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("version 1.0.2, by Doug Fraker, 2021.\n\nnesdoug.com");
        }
    }
}
