/*<FILE_LICENSE>
* NFX (.NET Framework Extension) Unistack Library
* Copyright 2003-2014 IT Adapter Inc / 2015 Aum Code LLC
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
</FILE_LICENSE>*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using NUnit.Framework;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.Distributed;
using NFX.DataAccess.MongoDB;
using NFX.Serialization.JSON;
using NFX.Financial;

namespace NFX.NUnit.Integration.MongoDB
{
  [TestFixture]
  public class ConversionTests
  {

    [Test]
    public void T_01_Equals()
    {
      var row = new RowA
      {
        String1 = "Mudaker", String2 = null,
        Date1 = new DateTime(1980, 07, 12), Date2 = null,
        Bool1 = true, Bool2 = null,
        Guid1 = new Guid("{9195F7DB-FE21-4BB2-B006-2496F4E24D14}"), Guid2 = null,
        Gdid1 = new GDID(0, 12345), Gdid2 = null,
        Float1 = 127.0123f, Float2 = null,
        Double1 = 122345.012d, Double2 = null,
        Decimal1 = 1234567.098M, Decimal2 = null,
        Amount1 = new Amount("din", 123.11M), Amount2 = null
      };
        
      var rc = new RowConverter();
      
      var doc = rc.RowToBSONDocument( row, "A" );

      Console.WriteLine(doc.ToString());

      var row2 = new RowA();
      rc.BSONDocumentToRow(doc, row2, "A");

      Assert.IsTrue( row.Equals( row2 ) );
    }

    [Test]
    public void T_02_Manual()
    {
      var row = new RowA
      {
        String1 = "Mudaker", String2 = null,
        Date1 = new DateTime(1980, 07, 12), Date2 = null,
        Bool1 = true, Bool2 = null,
        Guid1 = new Guid("{9195F7DB-FE21-4BB2-B006-2496F4E24D14}"), Guid2 = null,
        Gdid1 = new GDID(0, 12345), Gdid2 = null,
        Float1 = 127.0123f, Float2 = null,
        Double1 = 122345.012d, Double2 = null,
        Decimal1 = 1234567.098M, Decimal2 = null,
        Amount1 = new Amount("din", 123.11M), Amount2 = null
      };
        
      var rc = new RowConverter();
      
      var doc = rc.RowToBSONDocument( row, "A" );

      Console.WriteLine(doc.ToString());

      var row2 = new RowA();
      rc.BSONDocumentToRow(doc, row2, "A");

      Assert.AreEqual("Mudaker", row2.String1);
      Assert.IsNull( row2.String2);
      Assert.IsTrue( row2.Bool1 );
      Assert.IsNull( row2.Bool2);
      Assert.AreEqual(new Guid("{9195F7DB-FE21-4BB2-B006-2496F4E24D14}"), row2.Guid1);
      Assert.IsNull( row2.Guid2);
      Assert.AreEqual(new GDID(0, 12345), row2.Gdid1);
      Assert.IsNull( row2.Gdid2);
      Assert.AreEqual(127.0123f, row2.Float1);
      Assert.IsNull( row2.Float2);
      Assert.AreEqual(122345.012d, row2.Double1);
      Assert.IsNull( row2.Double2);
      Assert.AreEqual(1234567.098M, row2.Decimal1);
      Assert.IsNull( row2.Decimal2);
      Assert.AreEqual(new Amount("din", 123.11M), row2.Amount1);
      Assert.IsNull( row2.Amount2);
    }


    [Test]
    public void T_03_Manual_wo_NULLs()
    {
      var row = new RowA
      {
        String1 = "Mudaker", String2 = "Kapernik",
        Date1 = new DateTime(1980, 07, 12), Date2 = new DateTime(1680, 12, 11),
        Bool1 = false, Bool2 = true,
        Guid1 = new Guid("{9195F7DB-FE21-4BB2-B006-2496F4E24D14}"), Guid2 = new Guid("{BABACACA-FE21-4BB2-B006-2496F4E24D14}"),
        Gdid1 = new GDID(3, 12345), Gdid2 = new GDID(4, 1212345),
        Float1 = 127.0123f, Float2 = -0.123f,
        Double1 = 122345.012d, Double2 = -12345.11f,
        Decimal1 = 1234567.098M, Decimal2 = 22m,
        Amount1 = new Amount("din", 123.11M), Amount2 = new Amount("din", 8901234567890.012M)
      };
        
      var rc = new RowConverter();
      
      var doc = rc.RowToBSONDocument( row, "A" );

      Console.WriteLine(doc.ToString());

      var row2 = new RowA();
      rc.BSONDocumentToRow(doc, row2, "A");

      Assert.AreEqual("Mudaker", row2.String1);
      Assert.AreEqual("Kapernik", row2.String2);
      Assert.IsFalse( row2.Bool1 );
      Assert.IsTrue( row2.Bool2.Value );
      Assert.AreEqual(new Guid("{9195F7DB-FE21-4BB2-B006-2496F4E24D14}"), row2.Guid1);
      Assert.AreEqual(new Guid("{BABACACA-FE21-4BB2-B006-2496F4E24D14}"), row2.Guid2);
      Assert.AreEqual(new GDID(3, 12345), row2.Gdid1);
      Assert.AreEqual(new GDID(4, 1212345), row2.Gdid2);
      Assert.AreEqual(127.0123f, row2.Float1);
      Assert.AreEqual(-0.123f, row2.Float2);
      Assert.AreEqual(122345.012d, row2.Double1);
      Assert.AreEqual(-12345.11f, row2.Double2);
      Assert.AreEqual(1234567.098M, row2.Decimal1);
      Assert.AreEqual(22m, row2.Decimal2);
      Assert.AreEqual(new Amount("din", 123.11M), row2.Amount1);
      Assert.AreEqual(new Amount("din", 8901234567890.012M), row2.Amount2);
    }



    [Test]
    public void T_04_Targeting()
    {
      var row = new RowA
      {
        String1 = "Mudaker", String2 = "Someone",
      };
        
      var rc = new RowConverter();
      
      var doc = rc.RowToBSONDocument( row, "A" );
      Console.WriteLine(doc.ToString());
      Assert.AreEqual( "Someone", doc["s2"].ToString());

      doc = rc.RowToBSONDocument( row, "B" );
      Console.WriteLine(doc.ToString());
      Assert.AreEqual( "Someone", doc["STRING-2"].ToString());


      doc = rc.RowToBSONDocument( row, "NonExistent" );
      Console.WriteLine(doc.ToString());
      Assert.AreEqual( "Someone", doc["String2"].ToString());
    }


    [Test]
    public void T_05_WithInnerRows()
    {
      var row = new RowB
      {
        Row1 = new RowA
      {
        String1 = "Mudaker", String2 = null,
        Date1 = new DateTime(1980, 07, 12), Date2 = null,
        Bool1 = true, Bool2 = null,
        Guid1 = new Guid("{9195F7DB-FE21-4BB2-B006-2496F4E24D14}"), Guid2 = null,
        Gdid1 = new GDID(0, 12345), Gdid2 = null,
        Float1 = 127.0123f, Float2 = null,
        Double1 = 122345.012d, Double2 = null,
        Decimal1 = 1234567.098M, Decimal2 = null,
        Amount1 = new Amount("din", 123.11M), Amount2 = null
      },
        Row2= new RowA
      {
        String1 = "Abraham ILyach Lincoln", String2 = "I know that I know nothing",
        Date1 = new DateTime(1877, 01, 02), Date2 = new DateTime(1977, 03, 15),
        Bool1 = false, Bool2 = true,
        Guid1 = new Guid("{AAAAAAAA-FE21-4BB2-B006-2496F4E24D14}"), Guid2 = null,
        Gdid1 = new GDID(0, 12323423423), Gdid2 =  new GDID(0, 187760098292476423),
        Float1 = 127.0123f, Float2 = 123.2f,
        Double1 = 122345.012d, Double2 = -18293f,
        Decimal1 = 1234567.098M, Decimal2 = -2312m,
        Amount1 = new Amount("usd", 89123M), Amount2 = new Amount("usd", 12398933.123m)
      }
      };
        
      var rc = new RowConverter();
      
      var doc = rc.RowToBSONDocument( row, "A" );

      Console.WriteLine(doc.ToString());

      var row2 = new RowB();
      rc.BSONDocumentToRow(doc, row2, "A");

      Assert.IsTrue( row.Equals( row2 ) );
    }

    [Test]
    public void T_06_TargetingInnerRows()
    {
      var row = new RowB{
       Row1 = new RowA  { String1 = "Mudaker", String2 = "Someone"},
       Row2 = new RowA  { String1 = "Zar", String2 = "Boris"}
      };
        
      var rc = new RowConverter();
      
      var doc = rc.RowToBSONDocument( row, "A" );
      Console.WriteLine(doc.ToString());
      Assert.AreEqual( "Someone", doc["Row1"]["s2"].ToString());
      Assert.AreEqual( "Boris", doc["Row2"]["s2"].ToString());

      doc = rc.RowToBSONDocument( row, "B" );
      Console.WriteLine(doc.ToString());
      Assert.AreEqual( "Someone", doc["Row1"]["STRING-2"].ToString());
      Assert.AreEqual( "Boris", doc["Row2"]["STRING-2"].ToString());


      doc = rc.RowToBSONDocument( row, "NonExistent" );
      Console.WriteLine(doc.ToString());
      Assert.AreEqual( "Someone", doc["Row1"]["String2"].ToString());
      Assert.AreEqual( "Boris", doc["Row2"]["String2"].ToString());
    }



    [Test]
    public void T_07_ArraysListsAndMaps()
    {
      var row = new RowC
      {
        Map = new JSONDataMap{{"Name","Xerson"},{"Age",123}},
        List = new List<object>{ 1,true, "YEZ!", -123.01m},
        ObjectArray = new object[]{123, -12, 789d, new GDID(0, 1223), null},
        MapArray = new JSONDataMap[]{ new JSONDataMap{{"a",1},{"b",true}},  new JSONDataMap{{"kosmos",234.12},{"b",null}} },
        MapList = new List<JSONDataMap>{ new JSONDataMap{{"abc",0},{"buba", new GDID(2, 1223)}},  new JSONDataMap{{"nothing",null}} }
      };

      var rc = new RowConverter();

      var doc = rc.RowToBSONDocument(row, "A");
      Console.WriteLine( doc.ToString() );

      var row2 = new RowC();
      rc.BSONDocumentToRow(doc, row2, "A");

      Assert.AreEqual(2, row2.Map.Count);
      Assert.AreEqual("Xerson", row2.Map["Name"]);
      Assert.AreEqual(123, row2.Map["Age"]);

      Assert.AreEqual(4, row2.List.Count);
      Assert.AreEqual(1, row2.List[0]);
      Assert.AreEqual(true, row2.List[1]);
      Assert.AreEqual("YEZ!", row2.List[2]);
      Assert.AreEqual(-123010m, row2.List[3]); //notice that "decimalness" is lost, because reading back into list<object>
      
      Assert.AreEqual(5, row2.ObjectArray.Length);
      Assert.AreEqual(123, row2.ObjectArray[0]);
      Assert.AreEqual(-12, row2.ObjectArray[1]);
      Assert.AreEqual(789, row2.ObjectArray[2]);
      Assert.IsTrue((new byte[]{0,0,0,0,0,0,0,0,0,0,0x04,0xc7}).SequenceEqual( (byte[])(row2.ObjectArray[3]) ));//notice that GDID is lost, it got turned into int because reading back in object[], so no type conversion happens
      Assert.AreEqual(null, row2.ObjectArray[4]);

      Assert.AreEqual(2, row2.MapArray.Length);
      Assert.AreEqual(1, row2.MapArray[0]["a"]);
      Assert.AreEqual(true, row2.MapArray[0]["b"]);
      Assert.AreEqual(234.12, row2.MapArray[1]["kosmos"]);
      Assert.AreEqual(null, row2.MapArray[1]["b"]);

      Assert.AreEqual(2, row2.MapList.Count);
      Assert.AreEqual(2, row2.MapList[0].Count);
      Assert.AreEqual(0, row2.MapList[0]["abc"]);
      Assert.IsTrue((new byte[]{0,0,0,2,0,0,0,0,0,0,0x04,0xc7}).SequenceEqual( (byte[])(row2.MapList[0]["buba"]) ) );//"GDIDness" is lost
      Assert.AreEqual(1, row2.MapList[1].Count);
      Assert.AreEqual(null, row2.MapList[1]["nothing"]);
    }



    [Test]
    public void T_08_VersionChange()
    {
      var rowA = new RowVersionA
      {
         FirstName = "Vladimir",
         LastName = "Lenin",
         Age =  DateTime.Now.Year - 1870
      };

      var rc = new RowConverter();

      var doc = rc.RowToBSONDocument(rowA, "A");

      Console.WriteLine( doc.ToString() );

      var rowB = new RowVersionB();

      rc.BSONDocumentToRow(doc, rowB, "MyLegacySystem");

      Assert.AreEqual("Vladimir", rowB.FirstName);
      Assert.AreEqual("Lenin", rowB.LastName);
      Assert.AreEqual(1870, rowB.DOB.Year);
    }




    [Test]
    public void T_09_DynamicRow()
    {
        var schema = new Schema("Dynamic Schema", 
              new Schema.FieldDef("ID", typeof(int), new List<FieldAttribute>{ new FieldAttribute(required: true, key: true)}),
              new Schema.FieldDef("Description", typeof(string), new List<FieldAttribute>{ new FieldAttribute(required: true)})
        );

        var row = new DynamicRow(schema);
            
        row["ID"] = 123;
        row["Description"] = "T-90 Tank";
     
        var rc = new RowConverter();
      
        var doc = rc.RowToBSONDocument( row, "A" );

        Console.WriteLine(doc.ToString());

        var row2 = new DynamicRow(schema);
        rc.BSONDocumentToRow(doc, row2, "A");

        Assert.AreEqual(123, row2["ID"]);
        Assert.AreEqual("T-90 Tank", row2["Description"]);
    }


    [Test]
    public void T_10_RowCycle_NoCycle()
    {
        var root = new RowCycle();

        root.SomeInt = 1234;
        root.InnerRow = new RowCycle();
        root.InnerRow.SomeInt = 567;
        root.InnerRow.InnerRow = null; //NO CYCLE!!!!
            
        var rc = new RowConverter();
      
        var doc = rc.RowToBSONDocument( root, "A" );

        Console.WriteLine(doc.ToString());

        var root2 = new RowCycle();
        rc.BSONDocumentToRow(doc, root2, "A");

        Assert.AreEqual(1234, root2.SomeInt);
        Assert.IsNotNull( root2.InnerRow );
        Assert.AreEqual(567, root2.InnerRow.SomeInt);
    }

    [Test]
    [ExpectedException(typeof(MongoDBDataAccessException), ExpectedMessage="reference cycle", MatchType=MessageMatch.Contains)]
    public void T_11_RowCycle_DirectCycle()
    {
        var root = new RowCycle();

        root.SomeInt = 1234;
        root.InnerRow = root; //Direct cycle
            
        var rc = new RowConverter();
      
        var doc = rc.RowToBSONDocument( root, "A" );  //exception
    }


    [Test]
    [ExpectedException(typeof(MongoDBDataAccessException), ExpectedMessage="reference cycle", MatchType=MessageMatch.Contains)]
    public void T_12_RowCycle_TransitiveCycle_1()
    {
        var root = new RowCycle();

        root.SomeInt = 1234;
        root.InnerRow = new RowCycle();
        root.InnerRow.SomeInt = 567;
        root.InnerRow.InnerRow = root; //TRANSITIVE(via another instance) CYCLE!!!!
            
        var rc = new RowConverter();
      
        var doc = rc.RowToBSONDocument( root, "A" );  //exception
    }

    [Test]
    [ExpectedException(typeof(MongoDBDataAccessException), ExpectedMessage="reference cycle", MatchType=MessageMatch.Contains)]
    public void T_13_RowCycle_TransitiveCycle_2()
    {
        var root = new RowCycle();

        root.SomeInt = 1234;
        root.InnerRow = new RowCycle();
        root.InnerRow.SomeInt = 567;
        root.InnerRow.InnerRow = new RowCycle();
        root.InnerRow.InnerRow.SomeInt = 890;
        root.InnerRow.InnerRow.InnerRow = root.InnerRow;  //TRANSITIVE(via another instance) CYCLE!!!!
            
        var rc = new RowConverter();
      
        var doc = rc.RowToBSONDocument( root, "A" );  //exception
    }


    [Test]
    [ExpectedException(typeof(MongoDBDataAccessException), ExpectedMessage="reference cycle", MatchType=MessageMatch.Contains)]
    public void T_14_RowCycle_TransitiveCycle_3()
    {
        var root = new JSONDataMap();

        root["a"] = 1;
        root["b"] = true;
        root["array"] = new JSONDataArray(){1,2,3,true,true,root};  //TRANSITIVE(via another instance) CYCLE!!!!
            
        var rc = new RowConverter(); 
      
        var doc = rc.ConvertCLRtoBSON(root, "A");//exception
    }


    [Test]
    public void T_15_BSONtoJSONDataMap()
    {
      var rowA = new RowVersionA
      {
         FirstName = "Vladimir",
         LastName = "Lenin",
         Age =  DateTime.Now.Year - 1870
      };

      var rc = new RowConverter();

      var doc = rc.RowToBSONDocument(rowA, "A");

      Console.WriteLine( doc.ToString() );

      var map = rc.BSONDocumentToJSONMap(doc);

      Assert.AreEqual(rowA.FirstName, map["FirstName"]); 
      Assert.AreEqual(rowA.LastName, map["LastName"]); 
      Assert.AreEqual(rowA.Age, map["Age"]); 
    }


    [Test]
    public void T_16_VersionChange_AmorphousDisabled()
    {
      var rowA = new RowVersionA
      {
         FirstName = "Vladimir",
         LastName = "Lenin",
         Age =  DateTime.Now.Year - 1870
      };

      var rc = new RowConverter();

      var doc = rc.RowToBSONDocument(rowA, "A", useAmorphousData: false);

      Console.WriteLine( doc.ToString() );

      var rowB = new RowVersionB();

      rc.BSONDocumentToRow(doc, rowB, "MyLegacySystem", useAmorphousData: false);

      Assert.AreEqual("Vladimir", rowB.FirstName);
      Assert.AreEqual("Lenin", rowB.LastName);
      Assert.AreEqual(new DateTime(), rowB.DOB);
    }

    [Test]
    public void T_17_VersionChange_AmorphousExtra()
    {
      var rowA = new RowVersionA
      {
         FirstName = "Vladimir",
         LastName = "Lenin",
         Age =  DateTime.Now.Year - 1870
      };

      rowA.AmorphousData["AABB"] = "extra data";

      var rc = new RowConverter();

      var doc = rc.RowToBSONDocument(rowA, "A", useAmorphousData: true);

      Console.WriteLine( doc.ToString() );

      var rowB = new RowVersionB();

      rc.BSONDocumentToRow(doc, rowB, "MyLegacySystem", useAmorphousData: true);

      Assert.AreEqual("Vladimir", rowB.FirstName);
      Assert.AreEqual("Lenin", rowB.LastName);
      Assert.AreEqual(1870, rowB.DOB.Year);
      Assert.AreEqual("extra data", rowB.AmorphousData["AABB"]);
    }


  }//Tests



         public class RowA : TypedRow
         {
           [Field] public string String1{get; set;}
           
           [Field(targetName: "A", backendName: "s2")] 
           [Field(targetName: "B", backendName: "STRING-2")] 
           public string String2{get; set;}
           
           [Field] public DateTime Date1{get; set;}
           [Field] public DateTime? Date2{get; set;}
           [Field] public bool Bool1{get; set;}
           [Field] public bool? Bool2{get; set;}
           [Field] public Guid Guid1{get; set;}
           [Field] public Guid? Guid2{get; set;}
           [Field] public GDID Gdid1{get; set;}
           [Field] public GDID? Gdid2{get; set;}

           [Field] public float Float1{get; set;}
           [Field] public float? Float2{get; set;}
           [Field] public double Double1{get; set;}
           [Field] public double? Double2{get; set;}
           [Field] public decimal Decimal1{get; set;}
           [Field] public decimal? Decimal2{get; set;}
           [Field] public Amount Amount1{get; set;}
           [Field] public Amount? Amount2{get; set;}

           public override bool Equals(Row other)
           {
             var or = other as RowA;
             if (or==null) return false;

             foreach(var f in this.Schema)
             {
               var v1 = this.GetFieldValue(f);
               var v2 = or.GetFieldValue(f);

               if (v1==null)
               {
                if (v2==null) continue;
                else return false;
               }

               if (!v1.Equals( v2 )) return false;
             }
             
             return true;
           }
         }


         public class RowB : TypedRow
         {
           [Field] public RowA Row1{get; set;}
           [Field] public RowA Row2{get; set;}
         }

         public class RowC : TypedRow
         {
           [Field] public JSONDataMap  Map{get; set;}
           [Field] public object[]  ObjectArray{get; set;}
           [Field] public JSONDataMap[]  MapArray{get; set;}
           [Field] public List<object> List{get; set;}
           [Field] public List<JSONDataMap> MapList{get; set;}
         }


         public class RowVersionA : AmorphousTypedRow
         {
           [Field] public string FirstName{get;set;}
           [Field] public string LastName{get;set;}
           [Field] public int Age{get;set;} //suppose we designed it a few years back and made a mistake - keeping an age as an INT (instread of a date)
         }

         public class RowVersionB : AmorphousTypedRow
         {
           [Field] public string FirstName{get;set;}
           [Field] public string LastName{get;set;}
           [Field] public DateTime DOB{get;set;}//today we made a new version of row with proper design - DOB date field
                                                // AFterLoad() allows to preserve existing data (as much as it can be done) 

           public int Age{ get { return (int)((DateTime.Now-DOB).TotalDays / 365d);}}//Age is now a calculated property, so existing code does not break
           
           public override void AfterLoad(string targetName)
           {
             if (targetName=="MyLegacySystem")//if data came from THIS system
             {
               if (AmorphousData.ContainsKey("Age"))//we take older format that was now placed in an amorphous dictionary
               {
                  var age = AmorphousData["Age"].AsInt();//convert it to int
                  this.DOB = DateTime.Now.AddYears(-age);//and init new stored property DOB, thus preserving meaningful data at least partially without data loss
               }
             }
           }
         }


         public class RowCycle : TypedRow
         {
           [Field] public int SomeInt{get; set;}
           [Field] public RowCycle InnerRow{get; set;}
         }


}
