using java.lang;
using JavaPort.Collections;
using System;
using System.Linq;
using System.Text;
using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;
using NullPointerException = java.lang.NullPointerException;
using java.util;

namespace java.io
{
public class ObjectOutputStream
    : OutputStream, ObjectOutput
{
    const short STREAM_MAGIC = unchecked((short)0xaced);
    const short STREAM_VERSION = 5;
    /* Each item in the stream is preceded by a tag
     */
    const byte TC_BASE = 0x70;
    const byte TC_NULL =         (byte)0x70;
    const byte TC_REFERENCE =    (byte)0x71;
    const byte TC_CLASSDESC =    (byte)0x72;
    const byte TC_OBJECT =       (byte)0x73;
    const byte TC_STRING =       (byte)0x74;
    const byte TC_ARRAY =        (byte)0x75;
    const byte TC_CLASS =        (byte)0x76;
    const byte TC_BLOCKDATA =    (byte)0x77;
    const byte TC_ENDBLOCKDATA = (byte)0x78;
    const byte TC_RESET =        (byte)0x79;
    const byte TC_BLOCKDATALONG= (byte)0x7A;
    const byte TC_EXCEPTION =    (byte)0x7B;
    const byte TC_LONGSTRING =   (byte)0x7C;
    const byte TC_PROXYCLASSDESC =       (byte)0x7D;
    const byte TC_ENUM =         (byte)0x7E;
    const byte TC_MAX =          (byte)0x7E;
    const int baseWireHandle = 0x7e0000;
    const byte SC_WRITE_METHOD = 0x01;
    const byte SC_BLOCK_DATA = 0x08;
    const byte SC_SERIALIZABLE = 0x02;
    const byte SC_EXTERNALIZABLE = 0x04;
    const byte SC_ENUM = 0x10;
    /* *******************************************************************/
    /* Security permissions */
    //const SerializablePermission SUBSTITUTION_PERMISSION =
    //                       new SerializablePermission("enableSubstitution");
    //const SerializablePermission SUBCLASS_IMPLEMENTATION_PERMISSION =
    //                new SerializablePermission("enableSubclassImplementation");
    public const int PROTOCOL_VERSION_1 = 1;
    public const int PROTOCOL_VERSION_2 = 2;

    /*
    private static class Caches {
        static readonly ConcurrentMap<WeakClassKey,Boolean> subclassAudits =
            new ConcurrentHashMap<WeakClassKey,Boolean>();
        static readonly ReferenceQueue<Type> subclassAuditsQueue =
            new ReferenceQueue<Type>(Type);
    }
    */
    private readonly BlockDataOutputStream bout;
    private readonly HandleTable handles;
    private readonly ReplaceTable subs;
    private int protocol = PROTOCOL_VERSION_2;
    //private int depth;
    //private byte[] primVals;
    private readonly boolean enableOverride;
    //private boolean enableReplace;
    // values below valid only during upcalls to writeObject()/writeExternal()
    //private SerialCallbackContext curContext;
    //private PutFieldImpl curPut;
    //private readonly DebugTraceInfoStack debugInfoStack;
    private static readonly boolean extendedDebugInfo = false;

    public ObjectOutputStream(OutputStream @out) {
        //verifySubclass();
        bout = new BlockDataOutputStream(@out);
        handles = new HandleTable(10, (float) 3.00);
        subs = new ReplaceTable(10, (float) 3.00);
        enableOverride = false;
        writeStreamHeader();
        bout.setBlockDataMode(true);
        //if (extendedDebugInfo) {
        //    debugInfoStack = new DebugTraceInfoStack();
        //} else {
        //    debugInfoStack = null;
        //}
    }

    protected ObjectOutputStream() {
        //SecurityManager sm = System.getSecurityManager();
        //if (sm != null) {
        //    sm.checkPermission(SUBCLASS_IMPLEMENTATION_PERMISSION);
        //}
        bout = null;
        handles = null;
        subs = null;
        enableOverride = true;
        //debugInfoStack = null;
    }

    public void useProtocolVersion(int version) {
        if (handles.size() != 0) {
            // REMIND: implement better check for pristine stream?
            throw new IllegalStateException("stream non-empty");
        }
        switch (version) {
            case PROTOCOL_VERSION_1:
            case PROTOCOL_VERSION_2:
                protocol = version;
                break;
            default:
                throw new IllegalArgumentException(
                    "unknown version: " + version);
        }
    }
    /*
    public void writeObject(Object obj) {
        if (enableOverride) {
            writeObjectOverride(obj);
            return;
        }
        try {
            writeObject0(obj, false);
        } catch (Exception ex) {
            if (depth == 0) {
                writeFatalException(ex);
            }
            throw ex;
        }
    }

    protected void writeObjectOverride(Object obj) {
    }
    public void writeUnshared(Object obj) {
        try {
            writeObject0(obj, true);
        } catch (Exception ex) {
            if (depth == 0) {
                writeFatalException(ex);
            }
            throw ex;
        }
    }

    public void defaultWriteObject() {
        if ( curContext == null ) {
            throw new Exception("not in call to writeObject");
        }
        Object curObj = curContext.getObj();
        ObjectStreamClass curDesc = curContext.getDesc();
        bout.setBlockDataMode(false);
        defaultWriteFields(curObj, curDesc);
        bout.setBlockDataMode(true);
    }

    public ObjectOutputStream.PutField putFields() {
        if (curPut == null) {
            if (curContext == null) {
                throw new Exception("not in call to writeObject");
            }
            Object curObj = curContext.getObj();
            ObjectStreamClass curDesc = curContext.getDesc();
            curPut = new PutFieldImpl(curDesc);
        }
        return curPut;
    }
    public void writeFields() {
        if (curPut == null) {
            throw new Exception("no current PutField object");
        }
        bout.setBlockDataMode(false);
        curPut.writeFields();
        bout.setBlockDataMode(true);
    }

    public void reset() {
        if (depth != 0) {
            throw new Exception("stream active");
        }
        bout.setBlockDataMode(false);
        bout.writeByte(TC_RESET);
        clear();
        bout.setBlockDataMode(true);
    }
    */

    protected void annotateClass(Type cl) {
    }

    protected void annotateProxyClass(Type cl) {
    }

    protected Object replaceObject(Object obj) {
        return obj;
    }
    /*
    protected boolean enableReplaceObject(boolean enable)
    {
        if (enable == enableReplace) {
            return enable;
        }
        if (enable) {
            SecurityManager sm = System.getSecurityManager();
            if (sm != null) {
                sm.checkPermission(SUBSTITUTION_PERMISSION);
            }
        }
        enableReplace = enable;
        return !enableReplace;
    }
    */
    protected void writeStreamHeader() {
        bout.writeShort(STREAM_MAGIC);
        bout.writeShort(STREAM_VERSION);
    }
    /*
    protected void writeClassDescriptor(ObjectStreamClass desc)
    {
        desc.writeNonProxy(this);
    }
    */
    public override void write(int val) {
        bout.write(val);
    }

    public override void write(byte[] buf) {
        bout.write(buf, 0, buf.Length, false);
    }

    public override void write(byte[] buf, int off, int len) {
        if (buf == null) {
            throw new NullPointerException();
        }
        int endoff = off + len;
        if (off < 0 || len < 0 || endoff > buf.Length || endoff < 0) {
            throw new Exception();
        }
        bout.write(buf, off, len, false);
    }

    public override void flush() {
        bout.flush();
    }

    protected void drain() {
        bout.drain();
    }

    public override void close() {
        flush();
        clear();
        bout.close();
    }

    public void writeBoolean(boolean val) {
        bout.writeBoolean(val);
    }

    public void writeByte(int val) {
        bout.writeByte(val);
    }

    public void writeShort(int val) {
        bout.writeShort(val);
    }

    public void writeChar(int val) {
        bout.writeChar(val);
    }

    public void writeInt(int val) {
        bout.writeInt(val);
    }

    /*
    public void writeLong(long val) {
        bout.writeLong(val);
    }
    public void writeFloat(float val) {
        bout.writeFloat(val);
    }
    public void writeDouble(double val) {
        bout.writeDouble(val);
    }
    */
    public void writeBytes(String str) {
        bout.writeBytes(str);
    }

    public void writeChars(String str) {
        bout.writeChars(str);
    }

    public void writeUTF(String str) {
        bout.writeUTF(str);
    }

    public abstract class PutField {
        public abstract void put(String name, boolean val);
        public abstract void put(String name, byte val);
        public abstract void put(String name, char val);
        public abstract void put(String name, short val);
        public abstract void put(String name, int val);
        public abstract void put(String name, long val);
        public abstract void put(String name, float val);
        public abstract void put(String name, double val);
        public abstract void put(String name, Object val);
        //@Deprecated
        public abstract void write(ObjectOutput @out);
    }

    int getProtocolVersion() {
        return protocol;
    }

    void writeTypeString(String str) {
        int handle;
        if (str == null) {
            writeNull();
        } else if ((handle = handles.lookup(str)) != -1) {
            writeHandle(handle);
        } else {
            writeString(str, false);
        }
    }
    
    /*
    private void verifySubclass() {
        Class cl = getClass();
        if (cl == typeof(ObjectOutputStream)) {
            return;
        }
        SecurityManager sm = System.getSecurityManager();
        if (sm == null) {
            return;
        }
        processQueue(Caches.subclassAuditsQueue, Caches.subclassAudits);
        WeakClassKey key = new WeakClassKey(cl, Caches.subclassAuditsQueue);
        Boolean result = Caches.subclassAudits.get(key);
        if (result == null) {
            result = Boolean.valueOf(auditSubclass(cl));
            Caches.subclassAudits.putIfAbsent(key, result);
        }
        if (result.booleanValue()) {
            return;
        }
        sm.checkPermission(SUBCLASS_IMPLEMENTATION_PERMISSION);
    }
    private static boolean auditSubclass(Class subcl) {
        Boolean result = AccessController.doPrivileged(
            new PrivilegedAction<Boolean>()// {
            //    public Boolean run() {
            //        for (Class cl = subcl;
            //             cl != ObjectOutputStream.class;
            //             cl = cl.getSuperclass())
            //        {
            //            try {
            //                cl.getDeclaredMethod(
            //                    "writeUnshared", new Class[] { Object.class });
            //                return Boolean.FALSE;
            //            } catch (NoSuchMethodException ex) {
            //            }
            //            try {
            //                cl.getDeclaredMethod("putFields", (Class[]) null);
            //                return Boolean.FALSE;
            //            } catch (NoSuchMethodException ex) {
            //            }
            //        }
            //        return Boolean.TRUE;
            //    }
            //}
        );
        return result.booleanValue();
    }
    */
    private void clear() {
        subs.clear();
        handles.clear();
    }
    /*
    private void writeObject0(Object obj, boolean unshared)
    {
        boolean oldMode = bout.setBlockDataMode(false);
        depth++;
        try {
            // handle previously written and non-replaceable objects
            int h;
            if ((obj = subs.lookup(obj)) == null) {
                writeNull();
                return;
            } else if (!unshared && (h = handles.lookup(obj)) != -1) {
                writeHandle(h);
                return;
            } else if (obj is Class) {
                writeClass((Class) obj, unshared);
                return;
            } else if (obj is ObjectStreamClass) {
                writeClassDesc((ObjectStreamClass) obj, unshared);
                return;
            }
            // check for replacement object
            Object orig = obj;
            Class cl = obj.getClass();
            ObjectStreamClass desc;
            for (;;) {
                // REMIND: skip this check for strings/arrays?
                Class repCl;
                desc = ObjectStreamClass.lookup(cl, true);
                if (!desc.hasWriteReplaceMethod() ||
                    (obj = desc.invokeWriteReplace(obj)) == null ||
                    (repCl = obj.getClass()) == cl)
                {
                    break;
                }
                cl = repCl;
            }
            if (enableReplace) {
                Object rep = replaceObject(obj);
                if (rep != obj && rep != null) {
                    cl = rep.getClass();
                    desc = ObjectStreamClass.lookup(cl, true);
                }
                obj = rep;
            }
            // if object replaced, run through original checks a second time
            if (obj != orig) {
                subs.assign(orig, obj);
                if (obj == null) {
                    writeNull();
                    return;
                } else if (!unshared && (h = handles.lookup(obj)) != -1) {
                    writeHandle(h);
                    return;
                } else if (obj is Class) {
                    writeClass((Class) obj, unshared);
                    return;
                } else if (obj is ObjectStreamClass) {
                    writeClassDesc((ObjectStreamClass) obj, unshared);
                    return;
                }
            }
            // remaining cases
            if (obj is String) {
                writeString((String) obj, unshared);
            } else if (cl.isArray()) {
                writeArray(obj, desc, unshared);
            } else if (obj is Enum) {
                writeEnum((Enum) obj, desc, unshared);
            } else if (obj is Serializable) {
                writeOrdinaryObject(obj, desc, unshared);
            } else {
                if (extendedDebugInfo) {
                    throw new NotSerializableException(
                        cl.getName() + "\n" + debugInfoStack.toString());
                } else {
                    throw new NotSerializableException(cl.getName());
                }
            }
        } finally {
            depth--;
            bout.setBlockDataMode(oldMode);
        }
    }
    */
    private void writeNull() {
        bout.writeByte(TC_NULL);
    }

    private void writeHandle(int handle) {
        bout.writeByte(TC_REFERENCE);
        bout.writeInt(baseWireHandle + handle);
    }
    /*
    private void writeClass(Class cl, boolean unshared) {
        bout.writeByte(TC_CLASS);
        writeClassDesc(ObjectStreamClass.lookup(cl, true), false);
        handles.assign(unshared ? null : cl);
    }

    private void writeClassDesc(ObjectStreamClass desc, boolean unshared)
    {
        int handle;
        if (desc == null) {
            writeNull();
        } else if (!unshared && (handle = handles.lookup(desc)) != -1) {
            writeHandle(handle);
        } else if (desc.isProxy()) {
            writeProxyDesc(desc, unshared);
        } else {
            writeNonProxyDesc(desc, unshared);
        }
    }
    */
    private boolean isCustomSubclass() {
        // Return true if this class is a custom subclass of ObjectOutputStream
        throw new NotImplementedException();
        //return getClass().getClassLoader()
        //           != ObjectOutputStream.class.getClassLoader();
    }
    /*
    private void writeProxyDesc(ObjectStreamClass desc, boolean unshared)
    {
        bout.writeByte(TC_PROXYCLASSDESC);
        handles.assign(unshared ? null : desc);
        Class cl = desc.forClass();
        Class[] ifaces = cl.getInterfaces();
        bout.writeInt(ifaces.length);
        for (int i = 0; i < ifaces.length; i++) {
            bout.writeUTF(ifaces[i].getName());
        }
        bout.setBlockDataMode(true);
        if (isCustomSubclass()) {
            ReflectUtil.checkPackageAccess(cl);
        }
        annotateProxyClass(cl);
        bout.setBlockDataMode(false);
        bout.writeByte(TC_ENDBLOCKDATA);
        writeClassDesc(desc.getSuperDesc(), false);
    }

    private void writeNonProxyDesc(ObjectStreamClass desc, boolean unshared)
    {
        bout.writeByte(TC_CLASSDESC);
        handles.assign(unshared ? null : desc);
        if (protocol == PROTOCOL_VERSION_1) {
            // do not invoke class descriptor write hook with old protocol
            desc.writeNonProxy(this);
        } else {
            writeClassDescriptor(desc);
        }
        Class cl = desc.forClass();
        bout.setBlockDataMode(true);
        if (isCustomSubclass()) {
            ReflectUtil.checkPackageAccess(cl);
        }
        annotateClass(cl);
        bout.setBlockDataMode(false);
        bout.writeByte(TC_ENDBLOCKDATA);
        writeClassDesc(desc.getSuperDesc(), false);
    }
    */
    private void writeString(String str, boolean unshared) {
        handles.assign(unshared ? null : str);
        long utflen = bout.getUTFLength(str);
        if (utflen <= 0xFFFF) {
            bout.writeByte(TC_STRING);
            bout.writeUTF(str, utflen);
        } else {
            throw new NotImplementedException();
            //bout.writeByte(TC_LONGSTRING);
            //bout.writeLongUTF(str, utflen);
        }
    }
    /*
    private void writeArray(Object array,
                            ObjectStreamClass desc,
                            boolean unshared)
    {
        bout.writeByte(TC_ARRAY);
        writeClassDesc(desc, false);
        handles.assign(unshared ? null : array);
        Class ccl = desc.forClass().getComponentType();
        if (ccl.isPrimitive()) {
            if (ccl == Integer.TYPE) {
                int[] ia = (int[]) array;
                bout.writeInt(ia.length);
                bout.writeInts(ia, 0, ia.length);
            } else if (ccl == Byte.TYPE) {
                byte[] ba = (byte[]) array;
                bout.writeInt(ba.length);
                bout.write(ba, 0, ba.length, true);
            } else if (ccl == Long.TYPE) {
                long[] ja = (long[]) array;
                bout.writeInt(ja.length);
                bout.writeLongs(ja, 0, ja.length);
            } else if (ccl == Float.TYPE) {
                float[] fa = (float[]) array;
                bout.writeInt(fa.length);
                bout.writeFloats(fa, 0, fa.length);
            } else if (ccl == Double.TYPE) {
                double[] da = (double[]) array;
                bout.writeInt(da.length);
                bout.writeDoubles(da, 0, da.length);
            } else if (ccl == Short.TYPE) {
                short[] sa = (short[]) array;
                bout.writeInt(sa.length);
                bout.writeShorts(sa, 0, sa.length);
            } else if (ccl == Character.TYPE) {
                char[] ca = (char[]) array;
                bout.writeInt(ca.length);
                bout.writeChars(ca, 0, ca.length);
            } else if (ccl == Boolean.TYPE) {
                boolean[] za = (boolean[]) array;
                bout.writeInt(za.length);
                bout.writeBooleans(za, 0, za.length);
            } else {
                throw new InternalError();
            }
        } else {
            Object[] objs = (Object[]) array;
            int len = objs.length;
            bout.writeInt(len);
            if (extendedDebugInfo) {
                debugInfoStack.push(
                    "array (class \"" + array.getClass().getName() +
                    "\", size: " + len  + ")");
            }
            try {
                for (int i = 0; i < len; i++) {
                    if (extendedDebugInfo) {
                        debugInfoStack.push(
                            "element of array (index: " + i + ")");
                    }
                    try {
                        writeObject0(objs[i], false);
                    } finally {
                        if (extendedDebugInfo) {
                            debugInfoStack.pop();
                        }
                    }
                }
            } finally {
                if (extendedDebugInfo) {
                    debugInfoStack.pop();
                }
            }
        }
    }
    private void writeEnum(Enum en,
                           ObjectStreamClass desc,
                           boolean unshared)
    {
        throw new NotImplementedException();
        //bout.writeByte(TC_ENUM);
        //ObjectStreamClass sdesc = desc.getSuperDesc();
        //writeClassDesc((sdesc.forClass() == Enum.class) ? desc : sdesc, false);
        //handles.assign(unshared ? null : en);
        //writeString(en.name(), false);
    }
    private void writeOrdinaryObject(Object obj,
                                     ObjectStreamClass desc,
                                     boolean unshared)
    {
        if (extendedDebugInfo) {
            debugInfoStack.push(
                (depth == 1 ? "root " : "") + "object (class \"" +
                obj.getClass().getName() + "\", " + obj.toString() + ")");
        }
        try {
            desc.checkSerialize();
            bout.writeByte(TC_OBJECT);
            writeClassDesc(desc, false);
            handles.assign(unshared ? null : obj);
            if (desc.isExternalizable() && !desc.isProxy()) {
                writeExternalData((Externalizable) obj);
            } else {
                writeSerialData(obj, desc);
            }
        } finally {
            if (extendedDebugInfo) {
                debugInfoStack.pop();
            }
        }
    }
    private void writeExternalData(Externalizable obj) {
        PutFieldImpl oldPut = curPut;
        curPut = null;
        if (extendedDebugInfo) {
            debugInfoStack.push("writeExternal data");
        }
        SerialCallbackContext oldContext = curContext;
        try {
            curContext = null;
            if (protocol == PROTOCOL_VERSION_1) {
                obj.writeExternal(this);
            } else {
                bout.setBlockDataMode(true);
                obj.writeExternal(this);
                bout.setBlockDataMode(false);
                bout.writeByte(TC_ENDBLOCKDATA);
            }
        } finally {
            curContext = oldContext;
            if (extendedDebugInfo) {
                debugInfoStack.pop();
            }
        }
        curPut = oldPut;
    }
    private void writeSerialData(Object obj, ObjectStreamClass desc)
    {
        ObjectStreamClass.ClassDataSlot[] slots = desc.getClassDataLayout();
        for (int i = 0; i < slots.length; i++) {
            ObjectStreamClass slotDesc = slots[i].desc;
            if (slotDesc.hasWriteObjectMethod()) {
                PutFieldImpl oldPut = curPut;
                curPut = null;
                SerialCallbackContext oldContext = curContext;
                if (extendedDebugInfo) {
                    debugInfoStack.push(
                        "custom writeObject data (class \"" +
                        slotDesc.getName() + "\")");
                }
                try {
                    curContext = new SerialCallbackContext(obj, slotDesc);
                    bout.setBlockDataMode(true);
                    slotDesc.invokeWriteObject(obj, this);
                    bout.setBlockDataMode(false);
                    bout.writeByte(TC_ENDBLOCKDATA);
                } finally {
                    curContext.setUsed();
                    curContext = oldContext;
                    if (extendedDebugInfo) {
                        debugInfoStack.pop();
                    }
                }
                curPut = oldPut;
            } else {
                defaultWriteFields(obj, slotDesc);
            }
        }
    }
    private void defaultWriteFields(Object obj, ObjectStreamClass desc)
    {
        // REMIND: perform conservative isInstance check here?
        desc.checkDefaultSerialize();
        int primDataSize = desc.getPrimDataSize();
        if (primVals == null || primVals.length < primDataSize) {
            primVals = new byte[primDataSize];
        }
        desc.getPrimFieldValues(obj, primVals);
        bout.write(primVals, 0, primDataSize, false);
        ObjectStreamField[] fields = desc.getFields(false);
        Object[] objVals = new Object[desc.getNumObjFields()];
        int numPrimFields = fields.length - objVals.length;
        desc.getObjFieldValues(obj, objVals);
        for (int i = 0; i < objVals.length; i++) {
            if (extendedDebugInfo) {
                debugInfoStack.push(
                    "field (class \"" + desc.getName() + "\", name: \"" +
                    fields[numPrimFields + i].getName() + "\", type: \"" +
                    fields[numPrimFields + i].getType() + "\")");
            }
            try {
                writeObject0(objVals[i],
                             fields[numPrimFields + i].isUnshared());
            } finally {
                if (extendedDebugInfo) {
                    debugInfoStack.pop();
                }
            }
        }
    }
    private void writeFatalException(Exception ex) {
        clear();
        boolean oldMode = bout.setBlockDataMode(false);
        try {
            bout.writeByte(TC_EXCEPTION);
            writeObject0(ex, false);
            clear();
        } finally {
            bout.setBlockDataMode(oldMode);
        }
    }
    */
    // REMIND: remove once hotspot inlines Float.floatToIntBits
    //private static native void floatsToBytes(float[] src, int srcpos,
    //                                         byte[] dst, int dstpos,
    //                                         int nfloats);
    //// REMIND: remove once hotspot inlines Double.doubleToLongBits
    //private static native void doublesToBytes(double[] src, int srcpos,
    //                                          byte[] dst, int dstpos,
    //                                          int ndoubles);
    /*
    private class PutFieldImpl : PutField {
        private readonly ObjectStreamClass desc;
        private readonly byte[] primVals;
        private readonly Object[] objVals;
        PutFieldImpl(ObjectStreamClass desc) {
            this.desc = desc;
            primVals = new byte[desc.getPrimDataSize()];
            objVals = new Object[desc.getNumObjFields()];
        }
        public void put(String name, boolean val) {
            Bits.putBoolean(primVals, getFieldOffset(name, Boolean.TYPE), val);
        }
        public void put(String name, byte val) {
            primVals[getFieldOffset(name, Byte.TYPE)] = val;
        }
        public void put(String name, char val) {
            Bits.putChar(primVals, getFieldOffset(name, Character.TYPE), val);
        }
        public void put(String name, short val) {
            Bits.putShort(primVals, getFieldOffset(name, Short.TYPE), val);
        }
        public void put(String name, int val) {
            Bits.putInt(primVals, getFieldOffset(name, Integer.TYPE), val);
        }
        public void put(String name, float val) {
            Bits.putFloat(primVals, getFieldOffset(name, Float.TYPE), val);
        }
        public void put(String name, long val) {
            Bits.putLong(primVals, getFieldOffset(name, Long.TYPE), val);
        }
        public void put(String name, double val) {
            Bits.putDouble(primVals, getFieldOffset(name, Double.TYPE), val);
        }
        public void put(String name, Object val) {
            throw new NotImplementedException();
            //objVals[getFieldOffset(name, Object.class)] = val;
        }
        // deprecated in ObjectOutputStream.PutField
        public void write(ObjectOutput @out) {
            if (ObjectOutputStream.@this != @out) {
                throw new IllegalArgumentException("wrong stream");
            }
            @out.write(primVals, 0, primVals.length);
            ObjectStreamField[] fields = desc.getFields(false);
            int numPrimFields = fields.length - objVals.length;
            // REMIND: warn if numPrimFields > 0?
            for (int i = 0; i < objVals.length; i++) {
                if (fields[numPrimFields + i].isUnshared()) {
                    throw new Exception("cannot write unshared object");
                }
                @out.writeObject(objVals[i]);
            }
        }
        internal void writeFields() {
            bout.write(primVals, 0, primVals.length, false);
            ObjectStreamField[] fields = desc.getFields(false);
            int numPrimFields = fields.length - objVals.length;
            for (int i = 0; i < objVals.length; i++) {
                if (extendedDebugInfo) {
                    debugInfoStack.push(
                        "field (class \"" + desc.getName() + "\", name: \"" +
                        fields[numPrimFields + i].getName() + "\", type: \"" +
                        fields[numPrimFields + i].getType() + "\")");
                }
                try {
                    writeObject0(objVals[i],
                                 fields[numPrimFields + i].isUnshared());
                } finally {
                    if (extendedDebugInfo) {
                        debugInfoStack.pop();
                    }
                }
            }
        }
        private int getFieldOffset(String name, Class type) {
            ObjectStreamField field = desc.getField(name, type);
            if (field == null) {
                throw new IllegalArgumentException("no such field " + name +
                                                   " with type " + type);
            }
            return field.getOffset();
        }
    }
    */
    private class BlockDataOutputStream
        : OutputStream/*, DataOutput*/
    {
        private static readonly int MAX_BLOCK_SIZE = 1024;
        private static readonly int MAX_HEADER_SIZE = 5;
        private static readonly int CHAR_BUF_SIZE = 256;
        private readonly byte[] buf = new byte[MAX_BLOCK_SIZE];
        private readonly byte[] hbuf = new byte[MAX_HEADER_SIZE];
        private readonly char[] cbuf = new char[CHAR_BUF_SIZE];
        private boolean blkmode = false;
        private int pos = 0;
        private readonly OutputStream @out;
        private readonly DataOutputStream dout;
        internal BlockDataOutputStream(OutputStream @out) {
            this.@out = @out;
            dout = new DataOutputStream(this);
        }
        internal boolean setBlockDataMode(boolean mode) {
            if (blkmode == mode) {
                return blkmode;
            }
            drain();
            blkmode = mode;
            return !blkmode;
        }
        boolean getBlockDataMode() {
            return blkmode;
        }
        /* ----------------- generic output stream methods ----------------- */
        /*
         * The following methods are equivalent to their counterparts in
         * OutputStream, except that they partition written data into data
         * blocks when in block data mode.
         */
        public override void write(int b) {
            if (pos >= MAX_BLOCK_SIZE) {
                drain();
            }
            buf[pos++] = (byte) b;
        }
        public override void write(byte[] b) {
            write(b, 0, b.Length, false);
        }
        public override void write(byte[] b, int off, int len) {
            write(b, off, len, false);
        }
        public override void flush() {
            drain();
            @out.flush();
        }
        public override void close() {
            flush();
            @out.close();
        }
        internal void write(byte[] b, int off, int len, boolean copy)
        {
            if (!(copy || blkmode)) {           // write directly
                drain();
                @out.write(b, off, len);
                return;
            }
            while (len > 0) {
                if (pos >= MAX_BLOCK_SIZE) {
                    drain();
                }
                if (len >= MAX_BLOCK_SIZE && !copy && pos == 0) {
                    // avoid unnecessary copy
                    writeBlockHeader(MAX_BLOCK_SIZE);
                    @out.write(b, off, MAX_BLOCK_SIZE);
                    off += MAX_BLOCK_SIZE;
                    len -= MAX_BLOCK_SIZE;
                } else {
                    int wlen = Math.min(len, MAX_BLOCK_SIZE - pos);
                    System.Array.Copy(b, off, buf, pos, wlen);
                    pos += wlen;
                    off += wlen;
                    len -= wlen;
                }
            }
        }
        internal void drain() {
            if (pos == 0) {
                return;
            }
            if (blkmode) {
                writeBlockHeader(pos);
            }
            @out.write(buf, 0, pos);
            pos = 0;
        }
        private void writeBlockHeader(int len) {
            if (len <= 0xFF) {
                hbuf[0] = TC_BLOCKDATA;
                hbuf[1] = (byte) len;
                @out.write(hbuf, 0, 2);
            } else {
                hbuf[0] = TC_BLOCKDATALONG;
                Bits.putInt(hbuf, 1, len);
                @out.write(hbuf, 0, 5);
            }
        }
        /* ----------------- primitive data output methods ----------------- */
        /*
         * The following methods are equivalent to their counterparts in
         * DataOutputStream, except that they partition written data into data
         * blocks when in block data mode.
         */
        public void writeBoolean(boolean v) {
            if (pos >= MAX_BLOCK_SIZE) {
                drain();
            }
            Bits.putBoolean(buf, pos++, v);
        }
        public void writeByte(int v) {
            if (pos >= MAX_BLOCK_SIZE) {
                drain();
            }
            buf[pos++] = (byte) v;
        }
        public void writeChar(int v) {
            if (pos + 2 <= MAX_BLOCK_SIZE) {
                Bits.putChar(buf, pos, (char) v);
                pos += 2;
            } else {
                dout.writeChar(v);
            }
        }
        public void writeShort(int v) {
            if (pos + 2 <= MAX_BLOCK_SIZE) {
                Bits.putShort(buf, pos, (short) v);
                pos += 2;
            } else {
                dout.writeShort(v);
            }
        }
        public void writeInt(int v) {
            if (pos + 4 <= MAX_BLOCK_SIZE) {
                Bits.putInt(buf, pos, v);
                pos += 4;
            } else {
                dout.writeInt(v);
            }
        }
        /*
        public void writeFloat(float v) {
            if (pos + 4 <= MAX_BLOCK_SIZE) {
                Bits.putFloat(buf, pos, v);
                pos += 4;
            } else {
                dout.writeFloat(v);
            }
        }
        public void writeLong(long v) {
            if (pos + 8 <= MAX_BLOCK_SIZE) {
                Bits.putLong(buf, pos, v);
                pos += 8;
            } else {
                dout.writeLong(v);
            }
        }
        public void writeDouble(double v) {
            if (pos + 8 <= MAX_BLOCK_SIZE) {
                Bits.putDouble(buf, pos, v);
                pos += 8;
            } else {
                dout.writeDouble(v);
            }
        }
        */
        public void writeBytes(String s) {
            int endoff = s.length();
            int cpos = 0;
            int csize = 0;
            for (int off = 0; off < endoff; ) {
                if (cpos >= csize) {
                    cpos = 0;
                    csize = Math.min(endoff - off, CHAR_BUF_SIZE);
                    s.getChars(off, off + csize, cbuf, 0);
                }
                if (pos >= MAX_BLOCK_SIZE) {
                    drain();
                }
                int n = Math.min(csize - cpos, MAX_BLOCK_SIZE - pos);
                int stop = pos + n;
                while (pos < stop) {
                    buf[pos++] = (byte) cbuf[cpos++];
                }
                off += n;
            }
        }
        public void writeChars(String s) {
            int endoff = s.length();
            for (int off = 0; off < endoff; ) {
                int csize = Math.min(endoff - off, CHAR_BUF_SIZE);
                s.getChars(off, off + csize, cbuf, 0);
                writeChars(cbuf, 0, csize);
                off += csize;
            }
        }
        public void writeUTF(String s) {
            writeUTF(s, getUTFLength(s));
        }
        /* -------------- primitive data array output methods -------------- */
        /*
         * The following methods write @out spans of primitive data values.
         * Though equivalent to calling the corresponding primitive write
         * methods repeatedly, these methods are optimized for writing groups
         * of primitive data values more efficiently.
         */
        void writeBooleans(boolean[] v, int off, int len) {
            int endoff = off + len;
            while (off < endoff) {
                if (pos >= MAX_BLOCK_SIZE) {
                    drain();
                }
                int stop = Math.min(endoff, off + (MAX_BLOCK_SIZE - pos));
                while (off < stop) {
                    Bits.putBoolean(buf, pos++, v[off++]);
                }
            }
        }
        void writeChars(char[] v, int off, int len) {
            int limit = MAX_BLOCK_SIZE - 2;
            int endoff = off + len;
            while (off < endoff) {
                if (pos <= limit) {
                    int avail = (MAX_BLOCK_SIZE - pos) >> 1;
                    int stop = Math.min(endoff, off + avail);
                    while (off < stop) {
                        Bits.putChar(buf, pos, v[off++]);
                        pos += 2;
                    }
                } else {
                    dout.writeChar(v[off++]);
                }
            }
        }
        void writeShorts(short[] v, int off, int len) {
            int limit = MAX_BLOCK_SIZE - 2;
            int endoff = off + len;
            while (off < endoff) {
                if (pos <= limit) {
                    int avail = (MAX_BLOCK_SIZE - pos) >> 1;
                    int stop = Math.min(endoff, off + avail);
                    while (off < stop) {
                        Bits.putShort(buf, pos, v[off++]);
                        pos += 2;
                    }
                } else {
                    dout.writeShort(v[off++]);
                }
            }
        }
        void writeInts(int[] v, int off, int len) {
            int limit = MAX_BLOCK_SIZE - 4;
            int endoff = off + len;
            while (off < endoff) {
                if (pos <= limit) {
                    int avail = (MAX_BLOCK_SIZE - pos) >> 2;
                    int stop = Math.min(endoff, off + avail);
                    while (off < stop) {
                        Bits.putInt(buf, pos, v[off++]);
                        pos += 4;
                    }
                } else {
                    dout.writeInt(v[off++]);
                }
            }
        }
        /*
        void writeFloats(float[] v, int off, int len) {
            int limit = MAX_BLOCK_SIZE - 4;
            int endoff = off + len;
            while (off < endoff) {
                if (pos <= limit) {
                    int avail = (MAX_BLOCK_SIZE - pos) >> 2;
                    int chunklen = Math.min(endoff - off, avail);
                    floatsToBytes(v, off, buf, pos, chunklen);
                    off += chunklen;
                    pos += chunklen << 2;
                } else {
                    dout.writeFloat(v[off++]);
                }
            }
        }
        void writeLongs(long[] v, int off, int len) {
            int limit = MAX_BLOCK_SIZE - 8;
            int endoff = off + len;
            while (off < endoff) {
                if (pos <= limit) {
                    int avail = (MAX_BLOCK_SIZE - pos) >> 3;
                    int stop = Math.min(endoff, off + avail);
                    while (off < stop) {
                        Bits.putLong(buf, pos, v[off++]);
                        pos += 8;
                    }
                } else {
                    dout.writeLong(v[off++]);
                }
            }
        }
        */
        /*
        void writeDoubles(double[] v, int off, int len) {
            int limit = MAX_BLOCK_SIZE - 8;
            int endoff = off + len;
            while (off < endoff) {
                if (pos <= limit) {
                    int avail = (MAX_BLOCK_SIZE - pos) >> 3;
                    int chunklen = Math.min(endoff - off, avail);
                    doublesToBytes(v, off, buf, pos, chunklen);
                    off += chunklen;
                    pos += chunklen << 3;
                } else {
                    dout.writeDouble(v[off++]);
                }
            }
        }
        */
        internal long getUTFLength(String s) {
            int len = s.length();
            long utflen = 0;
            for (int off = 0; off < len; ) {
                int csize = Math.min(len - off, CHAR_BUF_SIZE);
                s.getChars(off, off + csize, cbuf, 0);
                for (int cpos = 0; cpos < csize; cpos++) {
                    char c = cbuf[cpos];
                    if (c >= 0x0001 && c <= 0x007F) {
                        utflen++;
                    } else if (c > 0x07FF) {
                        utflen += 3;
                    } else {
                        utflen += 2;
                    }
                }
                off += csize;
            }
            return utflen;
        }
        internal void writeUTF(String s, long utflen) {
            if (utflen > 0xFFFFL) {
                throw new Exception();
            }
            writeShort((int) utflen);
            if (utflen == (long) s.length()) {
                writeBytes(s);
            } else {
                writeUTFBody(s);
            }
        }
        /*
        void writeLongUTF(String s) {
            writeLongUTF(s, getUTFLength(s));
        }
        internal void writeLongUTF(String s, long utflen) {
            writeLong(utflen);
            if (utflen == (long) s.length()) {
                writeBytes(s);
            } else {
                writeUTFBody(s);
            }
        }
        */
        private void writeUTFBody(String s) {
            int limit = MAX_BLOCK_SIZE - 3;
            int len = s.length();
            for (int off = 0; off < len; ) {
                int csize = Math.min(len - off, CHAR_BUF_SIZE);
                s.getChars(off, off + csize, cbuf, 0);
                for (int cpos = 0; cpos < csize; cpos++) {
                    char c = cbuf[cpos];
                    if (pos <= limit) {
                        if (c <= 0x007F && c != 0) {
                            buf[pos++] = (byte) c;
                        } else if (c > 0x07FF) {
                            buf[pos + 2] = (byte) (0x80 | ((c >> 0) & 0x3F));
                            buf[pos + 1] = (byte) (0x80 | ((c >> 6) & 0x3F));
                            buf[pos + 0] = (byte) (0xE0 | ((c >> 12) & 0x0F));
                            pos += 3;
                        } else {
                            buf[pos + 1] = (byte) (0x80 | ((c >> 0) & 0x3F));
                            buf[pos + 0] = (byte) (0xC0 | ((c >> 6) & 0x1F));
                            pos += 2;
                        }
                    } else {    // write one byte at a time to normalize block
                        if (c <= 0x007F && c != 0) {
                            write(c);
                        } else if (c > 0x07FF) {
                            write(0xE0 | ((c >> 12) & 0x0F));
                            write(0x80 | ((c >> 6) & 0x3F));
                            write(0x80 | ((c >> 0) & 0x3F));
                        } else {
                            write(0xC0 | ((c >> 6) & 0x1F));
                            write(0x80 | ((c >> 0) & 0x3F));
                        }
                    }
                }
                off += csize;
            }
        }
    }
    private class HandleTable {
        /* number of mappings in table/next available handle */
        private int _size;
        /* size threshold determining when to expand hash spine */
        private int threshold;
        /* factor for computing size threshold */
        private readonly float loadFactor;
        /* maps hash value -> candidate handle value */
        private int[] spine;
        /* maps handle value -> next candidate handle value */
        private int[] next;
        /* maps handle value -> associated object */
        private Object[] objs;
        internal HandleTable(int initialCapacity, float loadFactor) {
            this.loadFactor = loadFactor;
            spine = new int[initialCapacity];
            next = new int[initialCapacity];
            objs = new Object[initialCapacity];
            threshold = (int) (initialCapacity * loadFactor);
            clear();
        }
        internal int assign(Object obj) {
            if (_size >= next.Length) {
                growEntries();
            }
            if (_size >= threshold) {
                growSpine();
            }
            insert(obj, _size);
            return _size++;
        }
        internal int lookup(Object obj) {
            if (_size == 0) {
                return -1;
            }
            int index = hash(obj) % spine.Length;
            for (int i = spine[index]; i >= 0; i = next[i]) {
                if (objs[i] == obj) {
                    return i;
                }
            }
            return -1;
        }
        internal void clear() {
            Arrays.fill(spine, -1);
            Arrays.fill(objs, 0, _size, null);
            _size = 0;
        }
        internal int size() {
            return _size;
        }
        private void insert(Object obj, int handle) {
            int index = hash(obj) % spine.Length;
            objs[handle] = obj;
            next[handle] = spine[index];
            spine[index] = handle;
        }
        private void growSpine() {
            spine = new int[(spine.Length << 1) + 1];
            threshold = (int) (spine.Length * loadFactor);
            Arrays.fill(spine, -1);
            for (int i = 0; i < _size; i++) {
                insert(objs[i], i);
            }
        }
        private void growEntries() {
            int newLength = (next.Length << 1) + 1;
            int[] newNext = new int[newLength];
            System.Array.Copy(next, 0, newNext, 0, _size);
            next = newNext;
            Object[] newObjs = new Object[newLength];
            System.Array.Copy(objs, 0, newObjs, 0, _size);
            objs = newObjs;
        }
        private int hash(Object obj) {
            return obj.GetHashCode()/*System.identityHashCode(obj)*/ & 0x7FFFFFFF;
        }
    }
    private class ReplaceTable {
        /* maps object -> index */
        private readonly HandleTable htab;
        /* maps index -> replacement object */
        private Object[] reps;
        internal ReplaceTable(int initialCapacity, float loadFactor) {
            htab = new HandleTable(initialCapacity, loadFactor);
            reps = new Object[initialCapacity];
        }
        void assign(Object obj, Object rep) {
            int index = htab.assign(obj);
            while (index >= reps.Length) {
                grow();
            }
            reps[index] = rep;
        }
        internal Object lookup(Object obj) {
            int index = htab.lookup(obj);
            return (index >= 0) ? reps[index] : obj;
        }
        internal void clear() {
            Arrays.fill(reps, 0, htab.size(), null);
            htab.clear();
        }
        int size() {
            return htab.size();
        }
        private void grow() {
            Object[] newReps = new Object[(reps.Length << 1) + 1];
            System.Array.Copy(reps, 0, newReps, 0, reps.Length);
            reps = newReps;
        }
    }
    /*
    private class DebugTraceInfoStack {
        private readonly List<String> stack;
        DebugTraceInfoStack() {
            stack = new ArrayList<String>();
        }
        void clear() {
            stack.clear();
        }
        void pop() {
            stack.remove(stack.size()-1);
        }
        void push(String entry) {
            stack.add("\t- " + entry);
        }
        public String toString() {
            StringBuilder buffer = new StringBuilder();
            if (!stack.isEmpty()) {
                for(int i = stack.size(); i > 0; i-- ) {
                    buffer.append(stack.get(i-1) + ((i != 1) ? "\n" : ""));
                }
            }
            return buffer.toString();
        }
    }
    */
}
}
