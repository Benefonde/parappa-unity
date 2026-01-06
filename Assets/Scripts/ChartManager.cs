using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


// worst code i have ever written <3
public abstract class SerializedDataThingy
{
    public int key;
    public object value;
    public abstract string GetAsString();
}

public class SerializedStringThingy : SerializedDataThingy
{
    public string value;
    public override string GetAsString()
    {
        return key + ":" + value;
    }
}
public class SerializedListThingy : SerializedDataThingy
{
    public SerializableObject[] value;

    public override string GetAsString()
    {
        string data = key + ":" + value.Length + "/";
        foreach (var item in value)
        {
            data += item.GetAsString();
            data += ";";
        }
        return data;
    }
}
public class SerializeReader
{
    string data;
    int offset;
    public SerializeReader(string theString)
    {
        data = theString;
        offset = 0;

    }
    public char Pop()
    {
        return data[offset++];
    }
    public char Peek()
    {
        return data[offset];
    }
    public bool HasMore()
    {
        if (offset + 1 >= data.Length)
        {
            return false;
        }
        return true;
    }
    public string ReadUntil(char stop)
    {
        string str = "";
        while (Peek() != stop)
        {
            str += Pop();
        }
        Pop(); // eat
        return str;
    }
    public string ReadStringThingy()
    {
        return ReadUntil(',');
    }
    public t[] ReadListThingy<t>() where t : SerializableObject, new()
    {
        int length = int.Parse(ReadUntil('/'));
        t[] list = new t[length];
        for (int i = 0; i < length; i++)
        {
            t thingy = new t();
            list[i] = thingy;
            thingy.LoadFromReader(this);
        }
        if (Peek() == ',') { Pop(); }
        return list;
    }
}
public abstract class SerializableObject
{
    public abstract SerializedDataThingy[] GetData();
    public string GetAsString()
    {
        string data = "";
        var itemData = GetData();
        foreach (var data1 in itemData)
        {
            data += data1.GetAsString() + ",";
        }
        return data;
    }
    public abstract void ReadKey(int key, SerializeReader reader);
    public void LoadFromReader(SerializeReader reader)
    {
        char letter = reader.HasMore() ? reader.Peek() : ';';
        while (letter != ';')
        {
            int key = int.Parse(reader.ReadUntil(':'));
            ReadKey(key, reader);
            letter = reader.HasMore() ? reader.Peek() : ';';
        }
        if (reader.HasMore())
        {
            reader.Pop();
        }
    }
}


public class Chart : SerializableObject
{
    public float bpm;
    public Line[] lines;
    public string chartName;

    public override SerializedDataThingy[] GetData()
    {
        SerializedDataThingy[] data = new SerializedDataThingy[3];
        data[0] = new SerializedStringThingy { key = 1, value = bpm.ToString(CultureInfo.InvariantCulture) };
        data[1] = new SerializedListThingy { key = 2, value = lines };
        data[2] = new SerializedStringThingy { key = 3, value = Convert.ToBase64String(Encoding.UTF8.GetBytes(chartName)) };
        return data;
    }

    public override void ReadKey(int key, SerializeReader reader)
    {
        switch (key)
        {
            case 1:
                bpm = float.Parse(reader.ReadStringThingy(), CultureInfo.InvariantCulture);
                break;
            case 2:
                lines = reader.ReadListThingy<Line>();
                break;
            case 3:
                chartName = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadStringThingy()));
                break;
        }
    }
}
public class Line : SerializableObject
{
    public byte length;
    public Button[] buttons;
    public string subtitle = string.Empty;

    public override SerializedDataThingy[] GetData()
    {
        SerializedDataThingy[] data = new SerializedDataThingy[3];
        data[0] = new SerializedStringThingy { key = 1, value = length.ToString() };
        data[1] = new SerializedListThingy { key = 2, value = buttons };
        data[2] = new SerializedStringThingy { key = 3, value = Convert.ToBase64String(Encoding.UTF8.GetBytes(subtitle)) };
        return data;
    }
    public override void ReadKey(int key, SerializeReader reader)
    {
        switch (key)
        {
            case 1:
                length = byte.Parse(reader.ReadStringThingy());
                break;
            case 2:
                buttons = reader.ReadListThingy<Button>();
                break;
            case 3:
                subtitle = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadStringThingy()));
                break;
        }
    }
}

public class Button : SerializableObject
{
    public byte owner; // 0 = teacher, 1 = parappa, 2 = sfx
    public byte type; // 0 tringle 1 circle 2 cross 3 square 4 L 5  R
    public float position;

    public override SerializedDataThingy[] GetData()
    {
        SerializedDataThingy[] data = new SerializedDataThingy[3];
        data[0] = new SerializedStringThingy { key = 1, value = owner.ToString() };
        data[1] = new SerializedStringThingy { key = 2, value = type.ToString() };
        data[2] = new SerializedStringThingy { key = 3, value = position.ToString(CultureInfo.InvariantCulture) };

        return data;
    }
    public override void ReadKey(int key, SerializeReader reader)
    {
        switch (key)
        {
            case 1:
                owner = byte.Parse(reader.ReadStringThingy());
                break;
            case 2:
                type = byte.Parse(reader.ReadStringThingy());
                break;
            case 3:
                position = float.Parse(reader.ReadStringThingy(), CultureInfo.InvariantCulture);
                break;
        }
    }
}
public class ChartManager : MonoBehaviour
{
    public string SaveChart(Chart chart)
    {
        var wow = chart.GetAsString();
        return wow;
    }
    public Chart LoadChart(string chartString)
    {
        Chart chart = new Chart();
        chart.LoadFromReader(new SerializeReader(chartString));
        return chart;
    }
}