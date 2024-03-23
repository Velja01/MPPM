using FTN.Common;
using FTN.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
        private NetworkModelGDAProxy gdaQueryProxy = null;
        private NetworkModelGDAProxy GdaQueryProxy
        {
            get
            {
                return gdaQueryProxy;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            if (gdaQueryProxy == null)
            {
                gdaQueryProxy = new NetworkModelGDAProxy("NetworkModelGDAEndpoint");
                try
                {
                    gdaQueryProxy.Open();
                }
                catch
                {

                }
            }
            comboBoxGID1.ItemsSource = GetAllGIDs();
            comboBox2.ItemsSource = GetAllCodes();
            comboBoxGID3.ItemsSource = GetAllGIDs();

            DataContext = this;
        }
        public List<long> GetAllGIDs()
        {
            int iteratorId = 0;
            List<long> ids = new List<long>();
            List<ModelCode> props = new List<ModelCode>();
            List<long> result = new List<long>();
            try
            {
                props.Add(ModelCode.IDOBJ_GID);
                int numberOfResources = 2;
                int resourcesLeft = 0;
                foreach (DMSType dMSType in Enum.GetValues(typeof(DMSType)))
                {
                    if (dMSType == DMSType.MASK_TYPE) continue;

                    iteratorId = GdaQueryProxy.GetExtentValues(modelResourcesDesc.GetModelCodeFromType(dMSType), props);
                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                    while (resourcesLeft > 0)
                    {
                        List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                        for (int i = 0; i < rds.Count; i++)
                        {
                            result.Add(rds[i].Id);
                        }

                        resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                    }
                    GdaQueryProxy.IteratorClose(iteratorId);
                }
            }
            catch { }
            return result;
        }

        public List<ModelCode> GetAllCodes()
        {
            List<ModelCode> pomocna = new List<ModelCode>();
            foreach (ModelCode modelCode in Enum.GetValues(typeof(ModelCode)))
            {
                short sh = (short)modelCode;
                var type = ModelCodeHelper.GetTypeFromModelCode(modelCode);
                if (type != 0 && sh == 0)
                {
                    pomocna.Add(modelCode);
                }
            }
            return pomocna;
        }

        #region GetValues
        private void ComboBoxGID1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxGID1.SelectedItem != null)
            {
                short type = ModelCodeHelper.ExtractTypeFromGlobalId((long)comboBoxGID1.SelectedItem);
                List<ModelCode> lista = modelResourcesDesc.GetAllPropertyIds((DMSType)type);
                listBox1.ItemsSource = lista;
            }
            checkBox1.IsChecked = false;
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            List<ModelCode> pomLista = new List<ModelCode>();
            foreach (ModelCode mc in listBox1.SelectedItems)
            {
                pomLista.Add(mc);
            }
            submit1.Text = GetValues((long)comboBoxGID1.SelectedItem, pomLista);
        }
        private string GetValues(long globalId, List<ModelCode> props)
        {
            ResourceDescription rd = null;
            string result = "";
            try
            {
                rd = GdaQueryProxy.GetValues(globalId, props);

                foreach (Property p in rd.Properties)
                {
                    result += String.Format($"{p.Id} = ");
                    if (p.Id == ModelCode.IDOBJ_GID)
                    {
                        result += String.Format("0x{0:x16}\n", p.AsLong());
                        continue;
                    }
                    switch (p.Type)
                    {
                        case PropertyType.String:
                            result += $"{p.AsString()}\n";
                            break;
                        case PropertyType.Float:
                            result += $"{p.AsFloat()}\n";
                            break;
                        case PropertyType.Int32:
                            result += $"{p.AsInt()}\n";
                            break;
                        case PropertyType.Bool:
                            result += $"{p.AsBool()}\n";
                            break;
                        case PropertyType.Enum:
                            result += $"{p.AsEnum()}\n";
                            break;
                        //case PropertyType.Enum:
                        //    if (p.Id == ModelCode.SWITCHINGOPERATION_NEWSTATE)
                        //        result += $"{(SwitchState)Enum.GetValues(typeof(SwitchState)).GetValue(p.AsInt())}\n";
                        //    else if (p.Id == ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPLIER || p.Id == ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPLIER)
                        //        result += $"{(UnitMultiplier)Enum.GetValues(typeof(UnitMultiplier)).GetValue(p.AsInt())}\n";
                        //    else
                        //        result += $"{(UnitSymbol)Enum.GetValues(typeof(UnitSymbol)).GetValue(p.AsInt())}\n";
                        //    break;
                        case PropertyType.DateTime:
                            result += $"{p.AsDateTime()}\n";
                            break;
                        case PropertyType.Reference:
                            result += String.Format("0x{0:x16}\n", p.AsReference());
                            break;
                        case PropertyType.ReferenceVector:
                            if (p.AsLongs().Count > 0)
                            {
                                string s = "";
                                foreach (var x in p.AsLongs())
                                {
                                    s += (String.Format("0x{0:x16}, \n", x));
                                }
                                result += s;
                            }
                            else { result += "empty\n"; }
                            break;
                        default:
                            result += $"{p.AsString()}\n";
                            break;
                    }
                }
            }
            catch { }
            return result;
        }
        #endregion

        #region GetExtentValues
        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                List<ModelCode> lista = modelResourcesDesc.GetAllPropertyIds((ModelCode)comboBox2.SelectedItem);
                listBox2.ItemsSource = lista;
            }
            checkBox2.IsChecked = false;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            List<ModelCode> pomLista = new List<ModelCode>();
            foreach (ModelCode mc in listBox2.SelectedItems)
            {
                pomLista.Add(mc);
            }
            submit2.Text = GetExtentValues((ModelCode)comboBox2.SelectedItem, pomLista);
        }

        private string GetExtentValues(ModelCode modelCode, List<ModelCode> props)
        {
            int iteratorId = 0;
            List<long> ids = new List<long>();
            string result = "";
            try
            {
                int numberOfResources = 2;
                int resourcesLeft = 0;

                iteratorId = GdaQueryProxy.GetExtentValues(modelCode, props);
                resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        foreach (Property p in rds[i].Properties)
                        {
                            result += String.Format($"{p.Id} = ");
                            if (p.Id == ModelCode.IDOBJ_GID)
                            {
                                result += String.Format("0x{0:x16}\n", p.AsLong());
                                continue;
                            }
                            switch (p.Type)
                            {
                                case PropertyType.String:
                                    result += $"{p.AsString()}\n";
                                    break;
                                case PropertyType.Float:
                                    result += $"{p.AsFloat()}\n";
                                    break;
                                case PropertyType.Int32:
                                    result += $"{p.AsInt()}\n";
                                    break;
                                case PropertyType.Bool:
                                    result += $"{p.AsBool()}\n";
                                    break;
                                case PropertyType.Enum:
                                    result += $"{p.AsEnum()}\n";
                                    break;
                                //case PropertyType.Enum:
                                //    if (p.Id == ModelCode.SWITCHINGOPERATION_NEWSTATE)
                                //        result += $"{(SwitchState)Enum.GetValues(typeof(SwitchState)).GetValue(p.AsInt())}\n";
                                //    else if (p.Id == ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPLIER || p.Id == ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPLIER)
                                //        result += $"{(UnitMultiplier)Enum.GetValues(typeof(UnitMultiplier)).GetValue(p.AsInt())}\n";
                                //    else
                                //        result += $"{(UnitSymbol)Enum.GetValues(typeof(UnitSymbol)).GetValue(p.AsInt())}\n";
                                //    break;
                                case PropertyType.DateTime:
                                    result += $"{p.AsDateTime()}\n";
                                    break;
                                case PropertyType.Reference:
                                    result += String.Format("0x{0:x16}\n", p.AsReference());
                                    break;
                                case PropertyType.ReferenceVector:
                                    if (p.AsLongs().Count > 0)
                                    {
                                        string s = "";
                                        foreach (var x in p.AsLongs())
                                        {
                                            s += (String.Format("0x{0:x16}, \n", x));
                                        }
                                        result += s;
                                    }
                                    else { result += "empty\n"; }
                                    break;
                                default:
                                    result += $"{p.AsString()}\n";
                                    break;
                            }
                        }
                        result += "\n";
                    }

                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }
                GdaQueryProxy.IteratorClose(iteratorId);
            }
            catch { }
            return result;
        }
        #endregion

        #region GetRelatedValues
        private void ComboBoxGID3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxGID3.SelectedItem != null)
            {
                short type = ModelCodeHelper.ExtractTypeFromGlobalId((long)comboBoxGID3.SelectedItem);
                List<ModelCode> lista = modelResourcesDesc.GetAllPropertyIds((DMSType)type);
                List<ModelCode> pomLista = new List<ModelCode>();
                foreach (ModelCode mc in lista)
                {
                    if (Property.GetPropertyType(mc) == PropertyType.Reference || Property.GetPropertyType(mc) == PropertyType.ReferenceVector)
                        pomLista.Add(mc);
                }
                if (comboBox32.Items.Count != 0)
                {
                    comboBox32.ItemsSource = null;
                    comboBox32.Items.Clear();
                }
                comboBox32.ItemsSource = null;
                comboBox32.Items.Clear();
                comboBox31.ItemsSource = pomLista;
                checkBox3.IsChecked = false;
            }
        }

        private void ComboBox31_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox31.SelectedItem != null)
            {
                string[] referenca = (comboBox31.SelectedItem.ToString()).Split('_');
                if (referenca[1].EndsWith("S"))
                {
                    referenca[1] = referenca[1].Remove(referenca[1].Length - 1);
                }

                ModelCode mc;
                if (String.Compare(referenca[0], "REGULARINTERVALSCHEDULE") == 0 && String.Compare(referenca[1], "TIMEPOINT") == 0)
                {
                    referenca[1] = "REGULARTIMEPOINT";
                }
                else if (String.Compare(referenca[0], "IRREGULARINTERVALSCHEDULE") == 0 && String.Compare(referenca[1], "TIMEPOINT") == 0)
                {
                    referenca[1] = "IRREGULARTIMEPOINT";
                }
                else if (String.Compare(referenca[0], "REGULARTIMEPOINT") == 0 && String.Compare(referenca[1], "INTERVALSCHEDULE") == 0)
                {
                    referenca[1] = "REGULARINTERVALSCHEDULE";
                }
                else if (String.Compare(referenca[0], "IRREGULARTIMEPOINT") == 0 && String.Compare(referenca[1], "INTERVALSCHEDULE") == 0)
                {
                    referenca[1] = "IRREGULARINTERVALSCHEDULE";
                }
                ModelCodeHelper.GetModelCodeFromString(referenca[1].ToString(), out mc);
                var type = ModelCodeHelper.GetTypeFromModelCode(mc);

                if (type == 0)
                {
                    if (comboBox32.Items.Count != 0)
                    {
                        comboBox32.ItemsSource = null;
                        comboBox32.Items.Clear();
                    }
                    //apstraktna klasa, treba naci konkretne
                    PronadjiKonkretneKlase(mc);
                }
                else
                {
                    if (comboBox32.Items.Count != 0)
                    {
                        comboBox32.ItemsSource = null;
                        comboBox32.Items.Clear();
                    }
                    comboBox32.ItemsSource = null;
                    comboBox32.Items.Clear();
                    comboBox32.Items.Add(mc);
                }
            }
        }
        private void PronadjiKonkretneKlase(ModelCode modelCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0x");
            string smc = string.Format("0x{0:x16}", (long)modelCode);
            char[] cs = ((smc.Split('x')[1]).ToCharArray());
            foreach (char c in cs)
            {
                if (c != '0') { sb.Append(c); }
                else { break; }
            }

            List<ModelCode> pomLista = new List<ModelCode>();
            foreach (ModelCode mc in Enum.GetValues(typeof(ModelCode)))
            {
                DMSType type = ModelCodeHelper.GetTypeFromModelCode(mc);
                if ((modelCode != mc) && (short)mc == 0 && type != 0)
                {
                    smc = string.Format("0x{0:x16}", (long)mc);
                    if (smc.StartsWith(sb.ToString()))
                        pomLista.Add(mc);
                }
            }

            comboBox32.ItemsSource = pomLista;
        }

        private void ComboBox32_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox32.SelectedItem != null)
            {
                List<ModelCode> lista = modelResourcesDesc.GetAllPropertyIds((ModelCode)comboBox32.SelectedItem);
                listBox3.ItemsSource = lista;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            long source = (long)comboBoxGID3.SelectedItem;
            Association association = new Association();
            association.PropertyId = (ModelCode)comboBox31.SelectedItem;
            association.Type = (ModelCode)comboBox32.SelectedItem;
            List<ModelCode> pomLista = new List<ModelCode>();
            foreach (ModelCode mc in listBox3.SelectedItems)
            {
                pomLista.Add(mc);
            }
            submit3.Text = GetRelatedValues(source, association, pomLista);
        }
        private string GetRelatedValues(long source, Association association, List<ModelCode> props)
        {
            List<long> resultIds = new List<long>();
            int numberOfResources = 2;

            string result = "";
            try
            {
                int iteratorId = GdaQueryProxy.GetRelatedValues(source, props, association);
                int resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = GdaQueryProxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        foreach (Property p in rds[i].Properties)
                        {
                            result += String.Format($"{p.Id} = ");
                            if (p.Id == ModelCode.IDOBJ_GID)
                            {
                                result += String.Format("0x{0:x16}\n", p.AsLong());
                                continue;
                            }
                            switch (p.Type)
                            {
                                case PropertyType.String:
                                    result += $"{p.AsString()}\n";
                                    break;
                                case PropertyType.Float:
                                    result += $"{p.AsFloat()}\n";
                                    break;
                                case PropertyType.Int32:
                                    result += $"{p.AsInt()}\n";
                                    break;
                                case PropertyType.Bool:
                                    result += $"{p.AsBool()}\n";
                                    break;
                                case PropertyType.Enum:
                                    result += $"{p.AsEnum()}\n";
                                    break;
                                //case PropertyType.Enum:
                                //    if(p.Id == ModelCode.SWITCHINGOPERATION_NEWSTATE)                                           
                                //        result += $"{(SwitchState)Enum.GetValues(typeof(SwitchState)).GetValue(p.AsInt())}\n";
                                //    else if(p.Id == ModelCode.BASICINTERVALSCHEDULE_VALUE1MULTIPLIER || p.Id == ModelCode.BASICINTERVALSCHEDULE_VALUE2MULTIPLIER)
                                //        result += $"{(UnitMultiplier)Enum.GetValues(typeof(UnitMultiplier)).GetValue(p.AsInt())}\n";
                                //    else
                                //        result += $"{(UnitSymbol)Enum.GetValues(typeof(UnitSymbol)).GetValue(p.AsInt())}\n";
                                //    break;
                                case PropertyType.DateTime:
                                    result += $"{p.AsDateTime()}\n";
                                    break;
                                case PropertyType.Reference:
                                    result += String.Format("0x{0:x16}\n", p.AsReference());
                                    break;
                                case PropertyType.ReferenceVector:
                                    if (p.AsLongs().Count > 0)
                                    {
                                        string s = "";
                                        foreach (var x in p.AsLongs())
                                        {
                                            s += (String.Format("0x{0:x16}, \n", x));
                                        }
                                        result += s;
                                    }
                                    else { result += "empty\n"; }
                                    break;
                                default:
                                    result += $"{p.AsString()}\n";
                                    break;
                            }
                        }
                        result += "\n";
                    }
                    resourcesLeft = GdaQueryProxy.IteratorResourcesLeft(iteratorId);
                }
                GdaQueryProxy.IteratorClose(iteratorId);
            }
            catch { }
            return result;
        }
        #endregion

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            listBox1.SelectAll();
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            listBox1.SelectedIndex = -1;
        }

        private void CheckBox2_Checked(object sender, RoutedEventArgs e)
        {
            listBox2.SelectAll();
        }

        private void CheckBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            listBox2.SelectedIndex = -1;
        }

        private void CheckBox3_Checked(object sender, RoutedEventArgs e)
        {
            listBox3.SelectAll();
        }

        private void CheckBox3_Unchecked(object sender, RoutedEventArgs e)
        {
            listBox3.SelectedIndex = -1;
        }
    }
}
