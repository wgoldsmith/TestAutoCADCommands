// (C) Copyright 2017 by  
//
using System;
using System.Windows.Forms;
using System.Drawing;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(TestAutoCADCommands.MyCommands))]

namespace TestAutoCADCommands
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        //[CommandMethod("stuuf", CommandFlags.Modal)]
        //public void MyCommand4()
        //{
        //    Database database = HostApplicationServices.WorkingDatabase;
        //    using (Transaction transaction = database.TransactionManager.StartTransaction())
        //    {
        //        SymbolTable symTable = (SymbolTable)transaction.GetObject(database.LinetypeTableId, OpenMode.ForRead);
        //        foreach (ObjectId id in symTable)
        //        {
        //            LinetypeTableRecord symbol = (LinetypeTableRecord)transaction.GetObject(id, OpenMode.ForRead);

        //            //TODO: Access to the symbol
        //            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(string.Format("\nName: {0}\nID: {1}", symbol.Name, symbol.ObjectId));
        //        }

        //        transaction.Commit();
        //    }
        //}

        [CommandMethod("ltest", CommandFlags.Modal)]
        public void MyCommand3() // This method can have any name
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;

            Editor ed = doc.Editor;

            string lineName = "Continuous"; // default value of Continuous

            LinetypeDialog ltd = new LinetypeDialog();

            // using will close connection when finished. Causes problems otherwise
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);

                System.Windows.Forms.DialogResult dr = ltd.ShowDialog();

                if (dr == System.Windows.Forms.DialogResult.OK)
                {

                    LinetypeTableRecord symbol = (LinetypeTableRecord)tr.GetObject(ltd.Linetype, OpenMode.ForRead);

                    if (!symbol.Name.Equals("ByLayer") && !symbol.Name.Equals("ByBlock"))
                    {
                        lineName = symbol.Name;
                    }

                    ed.WriteMessage("\nLinetype selected: " + symbol.Name + "\n");
                }
            }

            ed.Command(new Object[] { "-LAYER", "L", lineName, "", "" });
        }

        // Modal Command with localized name
        //[CommandMethod("coltest", CommandFlags.Modal)]
        //public void MyCommand() // This method can have any name
        //{
        //    //Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        //    //doc.SendStringToExecute("LAYDEL N stuff ", true, false, false);

        //    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

        //    Editor ed = doc.Editor;
            
        //    Autodesk.AutoCAD.Windows.ColorDialog dlg = new Autodesk.AutoCAD.Windows.ColorDialog();

        //    //if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        //    //{
        //    //    return;
        //    //}
        //    dlg.ShowDialog();

        //    ed.WriteMessage(dlg.ToString()+"\n");
        //    ed.WriteMessage(dlg.Color.ColorValue.R.ToString()+"\n");
        //    ed.WriteMessage(dlg.Color.ColorValue.ToString()+"\n");
        //    ed.WriteMessage(dlg.Color.ColorName+"\n");
        //    ed.WriteMessage(dlg.Color.ColorIndex.ToString());
        //}

        // Modal Command with localized name
        [CommandMethod("ctest", CommandFlags.Modal)]
        public void MyCommand2() // This method can have any name
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

            ed.Command(new Object[] { "-LAYER", "M", "NEWNAME", "" });

            Autodesk.AutoCAD.Windows.ColorDialog col = SelectColor();

            ChangeColor(col.Color.ColorValue.R.ToString(), col.Color.ColorValue.G.ToString(), col.Color.ColorValue.B.ToString());

            MyCommand3();
        }

        private Autodesk.AutoCAD.Windows.ColorDialog SelectColor()
        {
            Autodesk.AutoCAD.Windows.ColorDialog dlg = new Autodesk.AutoCAD.Windows.ColorDialog();

            dlg.ShowDialog();

            return dlg;
        }

        private void ChangeColor(string red, string green, string blue)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            ed.Command(new Object[] { "-LAYER", "C", "T", red + "," + green + "," + blue, "", "" });
        }
    }

}
