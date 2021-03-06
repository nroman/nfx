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
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;


using NFX.Inventorization;

namespace NFX.IO
{
    
    /// <summary>
    /// Writes primitives and other supported types to Slim-format stream. Use factory method of SlimFormat intance to create a new instance of SlimWriter class
    /// </summary>
    [Inventory(Concerns=SystemConcerns.Testing | SystemConcerns.MissionCriticality)]
    public class SlimWriter : WritingStreamer 
    {
        #region .ctor

            protected internal SlimWriter(Encoding encoding = null) : base(encoding)
            {
            }
        #endregion

        #region Fields


        #endregion


        #region Properties

            /// <summary>
            /// Returns SlimFormat that this writer implements
            /// </summary>
            public override StreamerFormat Format
            {
                get { return SlimFormat.Instance; }
            }


        #endregion



        #region Public


            public override void Flush()
            {
              m_Stream.Flush();
            }

         
         
          
        
          public override void Write(bool value)
          {
            m_Stream.WriteByte( value ? (byte)0xff : (byte)0);
          }

              public override void Write(bool? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
        

          public override void Write(byte value)
          {
            m_Stream.WriteByte(value);
          }

              public override void Write(byte? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  this.Write(value.Value);
                  return;
                }
                this.Write(false);
              }
        

          public override void Write(byte[] buffer)
          {
            if (buffer==null)
            {
              this.Write(false);
              return;
            }
            this.Write(true);
            var len = buffer.Length;
            if (len>SlimFormat.MAX_BYTE_ARRAY_LEN)
              throw new NFXIOException(StringConsts.SLIM_WRITE_BYTE_BUF_MAX_SIZE_ERROR.Args(len, SlimFormat.MAX_BYTE_ARRAY_LEN));

            this.Write(len);
            m_Stream.Write(buffer, 0, len);
          }
  
          public override void Write(char ch)
          {
            this.Write((short)ch);
          }
              public override void Write(char? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }


  
          public override void Write(char[] buffer)
          {
            if (buffer==null)
            {
              this.Write(false);
              return;
            }
            
            var buf = m_Encoding.GetBytes(buffer);
            this.Write(buf);
          }


          public override void Write(string[] array)
          {
            if (array==null)
            {
              this.Write(false);
              return;
            }
            this.Write(true);
            var len = array.Length;
            if (len>SlimFormat.MAX_STRING_ARRAY_CNT)
              throw new NFXIOException(StringConsts.SLIM_WRITE_STRING_ARRAY_MAX_SIZE_ERROR.Args(len, SlimFormat.MAX_STRING_ARRAY_CNT));

            this.Write(len);
            for(int i=0; i<len; i++)
             this.Write(array[i]); 
          }
          

          public override void Write(decimal value)
          {
            var bits = decimal.GetBits(value);
            this.Write( bits[0] );
            this.Write( bits[1] );
            this.Write( bits[2] );
            this.Write( bits[3] );
          }
              public override void Write(decimal? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
        

          public unsafe override void Write(double value)
          {
            ulong core = *(ulong*)(&value);
	          
            m_Buff32[0] = (byte)core;
	          m_Buff32[1] = (byte)(core >> 8);
	          m_Buff32[2] = (byte)(core >> 16);
	          m_Buff32[3] = (byte)(core >> 24);
	          m_Buff32[4] = (byte)(core >> 32);
	          m_Buff32[5] = (byte)(core >> 40);
	          m_Buff32[6] = (byte)(core >> 48);
	          m_Buff32[7] = (byte)(core >> 56);
	          
            m_Stream.Write(m_Buff32, 0, 8);
          }
  
              public override void Write(double? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }


          public unsafe override void Write(float value)
          {
            uint core = *(uint*)(&value);
	          m_Buff32[0] = (byte)core;
	          m_Buff32[1] = (byte)(core >> 8);
	          m_Buff32[2] = (byte)(core >> 16);
	          m_Buff32[3] = (byte)(core >> 24);
	          m_Stream.Write(m_Buff32, 0, 4);
          }

              public override void Write(float? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
        

          public override void Write(int value)
          {
            byte b = 0;
            
            if (value<0)
            {
             b = 1;
             value = ~value;//turn off minus bit but dont +1
            }
                       
            b = (byte)(b | ((value & 0x3f) << 1));
            value = value >> 6;
            var has = value != 0;
            if (has)
               b = (byte)(b | 0x80);
            m_Stream.WriteByte(b);
            while(has)
            {
              b = (byte)(value & 0x7f);
              value = value >> 7;
              has = value != 0;
              if (has)
               b = (byte)(b | 0x80);
              m_Stream.WriteByte(b);
            }
          }

              public override void Write(int? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }

          public override void Write(long value)
          {
            byte b = 0;
            
            if (value<0)
            {
             b = 1;
             value = ~value;//turn off minus bit but dont +1
            }
                       
            b = (byte)(b | ((value & 0x3f) << 1));
            value = value >> 6;
            var has = value != 0;
            if (has)
               b = (byte)(b | 0x80);
            m_Stream.WriteByte(b);
            while(has)
            {
              b = (byte)(value & 0x7f);
              value = value >> 7;
              has = value != 0;
              if (has)
               b = (byte)(b | 0x80);
              m_Stream.WriteByte(b);
            }
          }
              public override void Write(long? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }

        
          public override void Write(sbyte value)
          {
            m_Stream.WriteByte((byte)value); 
          }

              public override void Write(sbyte? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
        

          public override void Write(short value)
          {
            byte b = 0;
            
            if (value<0)
            {
             b = 1;
             value = (short)~value;//turn off minus bit but dont +1
            }
                       
            b = (byte)(b | ((value & 0x3f) << 1));
            value = (short)(value >> 6);
            var has = value != 0;
            if (has)
               b = (byte)(b | 0x80);
            m_Stream.WriteByte(b);
            while(has)
            {
              b = (byte)(value & 0x7f);
              value = (short)(value >> 7);
              has = value != 0;
              if (has)
               b = (byte)(b | 0x80);
              m_Stream.WriteByte(b);
            }
          }

              public override void Write(short? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
        
          public override void Write(string value)
          {
            if (value==null)
            {
              this.Write(false);
              return;
            }
            var buf = m_Encoding.GetBytes(value);
            this.Write(buf);
          }
         
          public override void Write(uint value)
          {
            var has = true;
            while(has)
            {
              byte b = (byte)(value & 0x7f);
              value = value >> 7;
              has = value != 0;
              if (has)
               b = (byte)(b | 0x80);
              m_Stream.WriteByte(b);
            } 
          }

              public override void Write(uint? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
        
          public override void Write(ulong value)
          {
            var has = true;
            while(has)
            {
              byte b = (byte)(value & 0x7f);
              value = value >> 7;
              has = value != 0;
              if (has)
               b = (byte)(b | 0x80);
              m_Stream.WriteByte(b);
            }
          }

              public override void Write(ulong? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }

        
          public override void Write(ushort value)
          {
            var has = true;
            while(has)
            {
              byte b = (byte)(value & 0x7f);
              value = (ushort)(value >> 7);
              has = value != 0;
              if (has)
               b = (byte)(b | 0x80);
              m_Stream.WriteByte(b);
            }
          }
              public override void Write(ushort? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }



          public override void Write(MetaHandle value)
          {
            var meta = !string.IsNullOrEmpty(value.Metadata);
           
            var handle = value.m_Handle; 

            byte b = 0;
            
            if (meta) b = 1;
                                    
            b = (byte)(b | ((handle & 0x3f) << 1));
            handle = (handle >> 6);
            var has = handle != 0;
            if (has)
               b = (byte)(b | 0x80);
            m_Stream.WriteByte(b);
            while(has)
            {
              b = (byte)(handle & 0x7f);
              handle = (handle >> 7);
              has = handle != 0;
              if (has)
               b = (byte)(b | 0x80);
              m_Stream.WriteByte(b);
            }

            if (meta)
             this.Write(value.Metadata);
          }

            
              public override void Write(MetaHandle? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }



        
          //public override void Write(byte[] buffer, int index, int count)
          //{
          //  m_Writer.Write(buffer, index, count);
          //}
        
          //public override void Write(char[] chars, int index, int count)
          //{
          //  m_Writer.Write(chars, index, count);
          //}


          public override void Write(DateTime value)
          {
            this.Write(value.ToBinary());
          }

              public override void Write(DateTime? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }


          public override void Write(TimeSpan value)
          {                       
            this.Write(value.Ticks);
          }

              public override void Write(TimeSpan? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }

          public override void Write(Guid value)
          {
            this.Write(value.ToByteArray());
          }

              public override void Write(Guid? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }

          public override void Write(NFX.DataAccess.Distributed.GDID value)
          {
            this.Write(value.Era);
            this.Write(value.ID);
          }

              public override void Write(NFX.DataAccess.Distributed.GDID? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
         

          public override void Write(NFX.Glue.Protocol.TypeSpec spec)
          {
             this.Write( spec.m_Name );
             m_Stream.WriteBEUInt64( spec.m_Hash );
          }

          public override void Write(NFX.Glue.Protocol.MethodSpec spec)
          {
             this.Write( spec.m_MethodName );
             m_Stream.WriteBEUInt64( spec.m_ReturnType );
             this.Write( spec.m_Signature );
             m_Stream.WriteBEUInt64( spec.m_Hash );
          }


          public override void Write(FID value)
          {
            m_Stream.WriteBEUInt64( value.ID );
          }

              public override void Write(FID? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }

          public override void Write(NFX.ApplicationModel.Pile.PilePointer value)
          {
            this.Write(value.NodeID);
            this.Write(value.Segment);
            this.Write(value.Address);
          }

              public override void Write(NFX.ApplicationModel.Pile.PilePointer? value)
              {
                if (value.HasValue)
                {
                  this.Write(true);
                  Write(value.Value);
                  return;
                }
                this.Write(false);
              }
         

        #endregion

    }
}
