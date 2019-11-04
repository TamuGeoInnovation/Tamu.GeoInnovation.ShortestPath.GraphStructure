using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace USC.GISResearchLab.ShortestPath.GraphStructure
{
    public enum KMLAltitudeMode : byte
    {
        Absolute,
        ClampToGround,
        RelativeToGround
    }

    public class KMLCoordinates
    {
        Double _Latitude = 0.0;
        Double _Longitude = 0.0;
        Double _Altitude = 0.0;

        public KMLCoordinates()
        { }

        public KMLCoordinates(Double Longitude, Double Latitude)
        {
            _Longitude = Longitude;
            _Latitude = Latitude;
        }

        public KMLCoordinates(Double Longitude, Double Latitude, Double Altitude)
        {
            _Longitude = Longitude;
            _Latitude = Latitude;
            _Altitude = Altitude;
        }

        public Double Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        public Double Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        public Double Altitude
        {
            get { return _Altitude; }
            set { _Altitude = value; }
        }

        public override bool Equals(object obj)
        {
            KMLCoordinates c = (KMLCoordinates)obj;
            if (this.Altitude.Equals(c.Altitude) && this.Latitude.Equals(c.Latitude) && this.Longitude.Equals(c.Longitude))
                return true;
            else
                return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }

    }

    public class KMLPoint
    {
        KMLCoordinates _Coords;
        KMLAltitudeMode _AltitudeMode = KMLAltitudeMode.ClampToGround;

        String _KMLFilePath = "C:\\tests\\";
        String _KMLFileName = "testp.kml";

        String _Name = "";
        String _Description = "";

        public KMLPoint()
        { }

        public KMLPoint(KMLCoordinates Coords)
        {
            _Coords = Coords;
        }

        public KMLPoint(KMLCoordinates Coords, String Name)
        {
            _Name = Name;
            _Coords = Coords;
        }

        public KMLPoint(KMLCoordinates Coords, String Name, String Description)
        {
            _Description = Description;
            _Name = Name;
            _Coords = Coords;
        }

        public KMLCoordinates Coords
        {
            get { return _Coords; }
            set { _Coords = value; }
        }

        public KMLAltitudeMode AltitudeMode
        {
            get { return _AltitudeMode; }
            set { _AltitudeMode = value; }
        }

        public string KMLFilePath
        {
            get { return _KMLFilePath; }
            set
            {
                _KMLFilePath = value;

                if (_KMLFilePath.Substring(_KMLFilePath.Length - 1, 1) != "\\")
                    _KMLFilePath += "\\";

            }
        }

        public string KMLFileName
        {
            get { return _KMLFileName; }
            set { _KMLFileName = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public void Save(string path, string fileName)
        {
            _KMLFilePath = path;
            _KMLFileName = fileName + ".kml";
            string FullFilePath = _KMLFilePath + _KMLFileName;

            XmlTextWriter xtr = new XmlTextWriter(FullFilePath, null);

            xtr.WriteStartDocument();

            xtr.WriteStartElement("kml");
            xtr.WriteString(" xmlns=\"http://earth.google.com/kml/2.1\"");
            {
                xtr.WriteStartElement("Document");
                {
                    xtr.WriteStartElement("Name");
                    xtr.WriteString(_Name);
                    xtr.WriteEndElement();//</name>

                    xtr.WriteStartElement("LookAt");
                    {
                        xtr.WriteStartElement("longitude");
                        xtr.WriteString(_Coords.Longitude.ToString());
                        xtr.WriteEndElement();//</longgitude>

                        xtr.WriteStartElement("latitude");
                        xtr.WriteString(_Coords.Latitude.ToString());
                        xtr.WriteEndElement();//</latitude>

                        xtr.WriteStartElement("range");
                        xtr.WriteString("400");
                        xtr.WriteEndElement();//</range>
                    }
                    xtr.WriteEndElement();//</Lookat>
                                          ///////////////////////////////////////////////////
                    xtr.WriteStartElement("Placemark");
                    {
                        xtr.WriteStartElement("Description");
                        xtr.WriteString("say something here!");
                        xtr.WriteEndElement();
                        xtr.WriteStartElement("name");
                        xtr.WriteString("name of the Placemark\n");
                        xtr.WriteString("long:  lat:");
                        xtr.WriteEndElement();

                        xtr.WriteStartElement("Point");
                        {
                            xtr.WriteStartElement("coordinates");
                            xtr.WriteString(_Coords.Longitude.ToString() + "," + _Coords.Latitude.ToString() + "," + _Coords.Altitude.ToString());
                            xtr.WriteEndElement();//</coordinates>
                        }
                        xtr.WriteEndElement();//</Point>
                    }
                    xtr.WriteEndElement();//</Placemark>
                                          /////////////////////////////////////////////////
                }
                xtr.WriteEndElement();//</Document>
            }
            xtr.WriteEndElement();//</kml>

            xtr.WriteEndDocument();//finishing writing the kml

            xtr.Close();

        }

    }//class KMLPoint


    public class KMLLine
    {
        Hashtable _Coords;
        LinkedList<Node> _Nodes;
        KMLAltitudeMode _AltitudeMode = KMLAltitudeMode.ClampToGround;

        String _KMLFilePath;
        String _KMLFileName;

        String _Name = "";
        String _Description = "";

        public KMLLine()
        { }

        public KMLLine(Hashtable Coords)
        {
            _Coords = Coords;
        }
        public KMLLine(LinkedList<Node> nodes)
        {
            _Nodes = nodes;
        }

        public KMLLine(Hashtable Coords, String Name)
        {
            _Name = Name;
            _Coords = Coords;
        }

        public KMLLine(Hashtable Coords, String Name, String Description)
        {
            _Description = Description;
            _Name = Name;
            _Coords = Coords;
        }

        public Hashtable Coords
        {
            get { return _Coords; }
            set { _Coords = value; }
        }

        public KMLAltitudeMode AltitudeMode
        {
            get { return _AltitudeMode; }
            set { _AltitudeMode = value; }
        }

        public string KMLFilePath
        {
            get { return _KMLFilePath; }
            set
            {
                _KMLFilePath = value;

                if (_KMLFilePath.Substring(_KMLFilePath.Length - 1, 1) != "\\")
                    _KMLFilePath += "\\";
            }
        }

        public string KMLFileName
        {
            get { return _KMLFileName; }
            set { _KMLFileName = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public void SaveAsPlacemarks(string fileName)
        {
            string FullFilePath = fileName;
            //just to make sure that everything is working ok!
            //if (pathByArcs.Length != _Coords.Count-1)
            //    return;
            XmlTextWriter xtr = new XmlTextWriter(FullFilePath, null);
            xtr.WriteStartDocument();

            xtr.WriteStartElement("kml");
            xtr.WriteString(" xmlns=\"http://earth.google.com/kml/2.1\"");
            {
                xtr.WriteStartElement("Document");
                {
                    xtr.WriteStartElement("Name");
                    xtr.WriteString(_Name);
                    xtr.WriteEndElement();//</name>

                    xtr.WriteStartElement("LookAt");
                    {
                        xtr.WriteStartElement("longitude");
                        xtr.WriteString(((KMLCoordinates)_Coords[0]).Longitude.ToString());
                        xtr.WriteEndElement();//</longgitude>

                        xtr.WriteStartElement("latitude");
                        xtr.WriteString(((KMLCoordinates)_Coords[0]).Latitude.ToString());
                        xtr.WriteEndElement();//</latitude>

                        xtr.WriteStartElement("range");
                        xtr.WriteString("400");
                        xtr.WriteEndElement();//</range>
                    }
                    xtr.WriteEndElement();//</Lookat>
                                          ///////////////////////////////////////////////////
                    for (int i = 0; i < _Coords.Count; i++)
                    {
                        xtr.WriteStartElement("Placemark");
                        {
                            xtr.WriteStartElement("name");
                            xtr.WriteString("Node" + i.ToString());
                            xtr.WriteEndElement();

                            //if (i < _Coords.Count-1)
                            //{
                            //    xtr.WriteStartElement("description");
                            //    xtr.WriteString(pathByArcs[i].Length.ToString());
                            //    xtr.WriteEndElement();
                            //}

                            xtr.WriteStartElement("Point");
                            {
                                xtr.WriteStartElement("coordinates");
                                xtr.WriteString(((KMLCoordinates)_Coords[i]).Longitude.ToString() + "," + ((KMLCoordinates)_Coords[i]).Latitude.ToString() + "," + ((KMLCoordinates)_Coords[i]).Altitude.ToString());
                                xtr.WriteEndElement();//</coordinates>
                            }
                            xtr.WriteEndElement();//</Point>
                        }
                        xtr.WriteEndElement();//</Placemark>
                    }
                    /////////////////////////////////////////////////
                }
                xtr.WriteEndElement();//</Document>
            }
            xtr.WriteEndElement();//</kml>

            xtr.WriteEndDocument();//finishing writing the kml

            xtr.Close();
        }
        public String ToKMLString()
        {
            StringBuilder kmlBuilder;
            //XmlTextWriter xtr = new XmlTextWriter(FullFilePath, null);
            kmlBuilder = new StringBuilder("<?xml version=\"1.0\"?>");

            kmlBuilder.Append("<kml xmlns=\"http://earth.google.com/kml/2.1\">");
            kmlBuilder.Append("<Placemark>");
            kmlBuilder.Append("<Name>");
            kmlBuilder.Append(_Name);
            kmlBuilder.Append("</Name>");

            kmlBuilder.Append("<Description>");
            kmlBuilder.Append(_Description);
            kmlBuilder.Append("</Description>");

            //Look At Region
            kmlBuilder.Append("<LookAt>");
            kmlBuilder.Append("<longitude>");
            kmlBuilder.Append(_Nodes.First.Value.Longitude.ToString());
            kmlBuilder.Append("</longitude>");
            kmlBuilder.Append("<latitude>");
            kmlBuilder.Append(_Nodes.First.Value.Latitude.ToString());
            kmlBuilder.Append("</latitude>");
            kmlBuilder.Append("<tilt>0</tilt><heading>0</heading>");
            kmlBuilder.Append("</LookAt>");

            //Style region
            kmlBuilder.Append("<Style><LineStyle><color>ffff00ff</color><width>4</width></LineStyle></Style>");

            //Line Coords
            kmlBuilder.Append("<LineString>");
            kmlBuilder.Append("<coordinates>");
            kmlBuilder.Append(GetCoordinateString());
            kmlBuilder.Append("</coordinates>");
            kmlBuilder.Append("</LineString>");
            kmlBuilder.Append("</Placemark>");
            kmlBuilder.Append("</kml>");
            return kmlBuilder.ToString();
        }

        private String GetCoordinateString()
        {
            StringBuilder sb = new StringBuilder();

            LinkedList<Node>.Enumerator en = _Nodes.GetEnumerator();
            while (en.MoveNext())
            {
                sb.Append(en.Current.Longitude.ToString() + ", " +
                  en.Current.Latitude.ToString() + " ");
            }
            return sb.ToString();

        }//private String GetCoordinateString()

    }//class KMLLine
}// namespace KMLUtls