using java.lang;
using JavaPort.Collections;
using System;
using System.Collections.Generic;
using System.IO;
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
public class ObjectInputStream
    : InputStream, ObjectInput
{
    const short STREAM_MAGIC = unchecked((short)0xaced);
    const short STREAM_VERSION = 5;
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

    public const int PROTOCOL_VERSION_1 = 1;
    public const int PROTOCOL_VERSION_2 = 2;

    private static readonly int NULL_HANDLE = -1;
    private static readonly Object unsharedMarker = new Object();
    //private static readonly HashMap<String, Type> primClasses
    //    = new HashMap<String, Type>(8, 1.0F);
    //static ObjectInputStream() {
    //    primClasses.put("boolean", boolean.class);
    //    primClasses.put("byte", byte.class);
    //    primClasses.put("char", char.class);
    //    primClasses.put("short", short.class);
    //    primClasses.put("int", int.class);
    //    primClasses.put("long", long.class);
    //    primClasses.put("float", float.class);
    //    primClasses.put("double", double.class);
    //    primClasses.put("void", void.class);
    //}
    //private static class Caches {
    //    static readonly ConcurrentMap<WeakClassKey,Boolean> subclassAudits =
    //        new ConcurrentHashMap<>();
    //    static readonly ReferenceQueue<Type> subclassAuditsQueue =
    //        new ReferenceQueue<>();
    //}
    private readonly BlockDataInputStream bin;
    //private readonly ValidationList vlist;
    private int depth = 0;
    private boolean closed;
    private readonly HandleTable handles;
    private int passHandle = NULL_HANDLE;
    private boolean defaultDataEnd = false;
//    private byte[] primVals;
    private readonly boolean enableOverride;
//    private boolean enableResolve;
    //private SerialCallbackContext curContext;
    public ObjectInputStream(InputStream @in) {
        //verifySubclass();
        bin = new BlockDataInputStream(@in);
        handles = new HandleTable(10);
        //vlist = new ValidationList();
        enableOverride = false;
        readStreamHeader();
        bin.setBlockDataMode(true);
    }
    protected ObjectInputStream() {
        //SecurityManager sm = System.getSecurityManager();
        //if (sm != null) {
        //    sm.checkPermission(SUBCLASS_IMPLEMENTATION_PERMISSION);
        //}
        bin = null;
        handles = null;
        //vlist = null;
        enableOverride = true;
    }
    //public Object readObject()
    //{
    //    if (enableOverride) {
    //        return readObjectOverride();
    //    }
    //    // if nested read, passHandle contains handle of enclosing object
    //    int outerHandle = passHandle;
    //    try {
    //        Object obj = readObject0(false);
    //        handles.markDependency(outerHandle, passHandle);
    //        Exception ex = handles.lookupException(passHandle);
    //        if (ex != null) {
    //            throw ex;
    //        }
    //        if (depth == 0) {
    //            vlist.doCallbacks();
    //        }
    //        return obj;
    //    } finally {
    //        passHandle = outerHandle;
    //        if (closed && depth == 0) {
    //            clear();
    //        }
    //    }
    //}
    protected Object readObjectOverride()
    {
        return null;
    }
    //public Object readUnshared() {
    //    // if nested read, passHandle contains handle of enclosing object
    //    int outerHandle = passHandle;
    //    try {
    //        Object obj = readObject0(true);
    //        handles.markDependency(outerHandle, passHandle);
    //        Exception ex = handles.lookupException(passHandle);
    //        if (ex != null) {
    //            throw ex;
    //        }
    //        if (depth == 0) {
    //            vlist.doCallbacks();
    //        }
    //        return obj;
    //    } finally {
    //        passHandle = outerHandle;
    //        if (closed && depth == 0) {
    //            clear();
    //        }
    //    }
    //}
    //public void defaultReadObject()
    //{
    //    if (curContext == null) {
    //        throw new Exception("not @in call to readObject");
    //    }
    //    Object curObj = curContext.getObj();
    //    ObjectStreamClass curDesc = curContext.getDesc();
    //    bin.setBlockDataMode(false);
    //    defaultReadFields(curObj, curDesc);
    //    bin.setBlockDataMode(true);
    //    if (!curDesc.hasWriteObjectData()) {
    //        /*
    //         * Fix for 4360508: since stream does not contain terminating
    //         * TC_ENDBLOCKDATA tag, set flag so that reading code elsewhere
    //         * knows to simulate end-of-custom-data behavior.
    //         */
    //        defaultDataEnd = true;
    //    }
    //    Exception ex = handles.lookupException(passHandle);
    //    if (ex != null) {
    //        throw ex;
    //    }
    //}

    //public ObjectInputStream.GetField readFields()
    //{
    //    if (curContext == null) {
    //        throw new Exception("not @in call to readObject");
    //    }
    //    Object curObj = curContext.getObj();
    //    ObjectStreamClass curDesc = curContext.getDesc();
    //    bin.setBlockDataMode(false);
    //    GetFieldImpl getField = new GetFieldImpl(curDesc);
    //    getField.readFields();
    //    bin.setBlockDataMode(true);
    //    if (!curDesc.hasWriteObjectData()) {
    //        /*
    //         * Fix for 4360508: since stream does not contain terminating
    //         * TC_ENDBLOCKDATA tag, set flag so that reading code elsewhere
    //         * knows to simulate end-of-custom-data behavior.
    //         */
    //        defaultDataEnd = true;
    //    }
    //    return getField;
    //}
    /*
    public void registerValidation(ObjectInputValidation obj, int prio)
    {
        if (depth == 0) {
            throw new Exception("stream inactive");
        }
        vlist.register(obj, prio);
    }
    protected Type resolveClass(ObjectStreamClass desc)
    {
        String name = desc.getName();
        try {
            return Class.forName(name, false, latestUserDefinedLoader());
        } catch (Exception ex) {
            Type cl = primClasses.get(name);
            if (cl != null) {
                return cl;
            } else {
                throw ex;
            }
        }
    }
    protected Type resolveProxyClass(String[] interfaces)
    {
        ClassLoader latestLoader = latestUserDefinedLoader();
        ClassLoader nonPublicLoader = null;
        boolean hasNonPublicInterface = false;
        // define proxy @in class loader of non-public interface(s), if any
        Class[] classObjs = new Class[interfaces.length];
        for (int i = 0; i < interfaces.length; i++) {
            Class cl = Class.forName(interfaces[i], false, latestLoader);
            if ((cl.getModifiers() & Modifier.PUBLIC) == 0) {
                if (hasNonPublicInterface) {
                    if (nonPublicLoader != cl.getClassLoader()) {
                        throw new IllegalAccessError(
                            "conflicting non-public interface class loaders");
                    }
                } else {
                    nonPublicLoader = cl.getClassLoader();
                    hasNonPublicInterface = true;
                }
            }
            classObjs[i] = cl;
        }
        try {
            return Proxy.getProxyClass(
                hasNonPublicInterface ? nonPublicLoader : latestLoader,
                classObjs);
        } catch (IllegalArgumentException e) {
            throw new Exception(null, e);
        }
    }
    */
    protected Object resolveObject(Object obj) {
        return obj;
    }
    //protected boolean enableResolveObject(boolean enable)
    //{
    //    if (enable == enableResolve) {
    //        return enable;
    //    }
    //    if (enable) {
    //        SecurityManager sm = System.getSecurityManager();
    //        if (sm != null) {
    //            sm.checkPermission(SUBSTITUTION_PERMISSION);
    //        }
    //    }
    //    enableResolve = enable;
    //    return !enableResolve;
    //}
    protected void readStreamHeader()
    {
        short s0 = bin.readShort();
        short s1 = bin.readShort();
        if (s0 != STREAM_MAGIC || s1 != STREAM_VERSION)
        {
            throw new Exception();
        }
    }
/*
    protected ObjectStreamClass readClassDescriptor()
    {
        ObjectStreamClass desc = new ObjectStreamClass();
        desc.readNonProxy(this);
        return desc;
    }
*/
    public override int read() {
        return bin.read();
    }
    public override int read(byte[] buf, int off, int len) {
        if (buf == null) {
            throw new NullPointerException();
        }
        int endoff = off + len;
        if (off < 0 || len < 0 || endoff > buf.Length || endoff < 0) {
            throw new Exception();
        }
        return bin.read(buf, off, len, false);
    }
    public override int available() {
        return bin.available();
    }
    public override void close() {
        /*
         * Even if stream already closed, propagate redundant close to
         * underlying stream to stay consistent with previous implementations.
         */
        closed = true;
        if (depth == 0) {
            clear();
        }
        bin.close();
    }
    public boolean readBoolean() {
        return bin.readBoolean();
    }
    public byte readByte() {
        return bin.readByte();
    }
    public int readUnsignedByte() {
        return bin.readUnsignedByte();
    }
    public char readChar() {
        return bin.readChar();
    }
    public short readShort() {
        return bin.readShort();
    }
    public int readUnsignedShort() {
        return bin.readUnsignedShort();
    }
    public int readInt() {
        return bin.readInt();
    }
    //public long readLong() {
    //    return bin.readLong();
    //}
    //public float readFloat() {
    //    return bin.readFloat();
    //}
    //public double readDouble() {
    //    return bin.readDouble();
    //}
    public void readFully(byte[] buf) {
        bin.readFully(buf, 0, buf.Length, false);
    }
    public void readFully(byte[] buf, int off, int len) {
        int endoff = off + len;
        if (off < 0 || len < 0 || endoff > buf.Length || endoff < 0) {
            throw new Exception();
        }
        bin.readFully(buf, off, len, false);
    }
    public int skipBytes(int len) {
        return bin.skipBytes(len);
    }
    //@Deprecated
    //public String readLine() {
    //    return bin.readLine();
    //}
    public String readUTF() {
        return bin.readUTF();
    }

    //public abstract class GetField {
    //    public abstract ObjectStreamClass getObjectStreamClass();
    //    public abstract boolean defaulted(String name);
    //    public abstract boolean get(String name, boolean val)
    //       ;
    //    public abstract byte get(String name, byte val);
    //    public abstract char get(String name, char val);
    //    public abstract short get(String name, short val);
    //    public abstract int get(String name, int val);
    //    public abstract long get(String name, long val);
    //    public abstract float get(String name, float val);
    //    public abstract double get(String name, double val);
    //    public abstract Object get(String name, Object val);
    //}
    //private void verifySubclass() {
    //    Class cl = getClass();
    //    if (cl == ObjectInputStream.class) {
    //        return;
    //    }
    //    SecurityManager sm = System.getSecurityManager();
    //    if (sm == null) {
    //        return;
    //    }
    //    processQueue(Caches.subclassAuditsQueue, Caches.subclassAudits);
    //    WeakClassKey key = new WeakClassKey(cl, Caches.subclassAuditsQueue);
    //    Boolean result = Caches.subclassAudits.get(key);
    //    if (result == null) {
    //        result = Boolean.valueOf(auditSubclass(cl));
    //        Caches.subclassAudits.putIfAbsent(key, result);
    //    }
    //    if (result.booleanValue()) {
    //        return;
    //    }
    //    sm.checkPermission(SUBCLASS_IMPLEMENTATION_PERMISSION);
    //}
    //private static boolean auditSubclass(final Type subcl) {
    //    Boolean result = AccessController.doPrivileged(
    //        new PrivilegedAction<Boolean>() {
    //            public Boolean run() {
    //                for (Type cl = subcl;
    //                     cl != ObjectInputStream.class;
    //                     cl = cl.getSuperclass())
    //                {
    //                    try {
    //                        cl.getDeclaredMethod(
    //                            "readUnshared", (Class[]) null);
    //                        return Boolean.FALSE;
    //                    } catch (NoSuchMethodException ex) {
    //                    }
    //                    try {
    //                        cl.getDeclaredMethod("readFields", (Class[]) null);
    //                        return Boolean.FALSE;
    //                    } catch (NoSuchMethodException ex) {
    //                    }
    //                }
    //                return Boolean.TRUE;
    //            }
    //        }
    //    );
    //    return result.booleanValue();
    //}
    private void clear() {
        handles.clear();
        //vlist.clear();
    }
    //private Object readObject0(boolean unshared) {
    //    boolean oldMode = bin.getBlockDataMode();
    //    if (oldMode) {
    //        int remain = bin.currentBlockRemaining();
    //        if (remain > 0) {
    //            throw new Exception();
    //        } else if (defaultDataEnd) {
    //            /*
    //             * Fix for 4360508: stream is currently at the end of a field
    //             * value block written via default serialization; since there
    //             * is no terminating TC_ENDBLOCKDATA tag, simulate
    //             * end-of-custom-data behavior explicitly.
    //             */
    //            throw new Exception();
    //        }
    //        bin.setBlockDataMode(false);
    //    }
    //    byte tc;
    //    while ((tc = bin.peekByte()) == TC_RESET) {
    //        bin.readByte();
    //        handleReset();
    //    }
    //    depth++;
    //    try {
    //        switch (tc) {
    //            case TC_NULL:
    //                return readNull();
    //            case TC_REFERENCE:
    //                return readHandle(unshared);
    //            case TC_CLASS:
    //                return readClass(unshared);
    //            case TC_CLASSDESC:
    //            case TC_PROXYCLASSDESC:
    //                return readClassDesc(unshared);
    //            case TC_STRING:
    //            case TC_LONGSTRING:
    //                return checkResolve(readString(unshared));
    //            case TC_ARRAY:
    //                return checkResolve(readArray(unshared));
    //            case TC_ENUM:
    //                return checkResolve(readEnum(unshared));
    //            case TC_OBJECT:
    //                return checkResolve(readOrdinaryObject(unshared));
    //            case TC_EXCEPTION:
    //                IOException ex = readFatalException();
    //                throw new WriteAbortedException("writing aborted", ex);
    //            case TC_BLOCKDATA:
    //            case TC_BLOCKDATALONG:
    //                if (oldMode) {
    //                    bin.setBlockDataMode(true);
    //                    bin.peek();             // force header read
    //                    throw new OptionalDataException(
    //                        bin.currentBlockRemaining());
    //                } else {
    //                    throw new StreamCorruptedException(
    //                        "unexpected block data");
    //                }
    //            case TC_ENDBLOCKDATA:
    //                if (oldMode) {
    //                    throw new OptionalDataException(true);
    //                } else {
    //                    throw new StreamCorruptedException(
    //                        "unexpected end of block data");
    //                }
    //            default:
    //                throw new StreamCorruptedException(
    //                    String.format("invalid type code: %02X", tc));
    //        }
    //    } finally {
    //        depth--;
    //        bin.setBlockDataMode(oldMode);
    //    }
    //}
    //private Object checkResolve(Object obj) {
    //    if (!enableResolve || handles.lookupException(passHandle) != null) {
    //        return obj;
    //    }
    //    Object rep = resolveObject(obj);
    //    if (rep != obj) {
    //        handles.setObject(passHandle, rep);
    //    }
    //    return rep;
    //}
    String readTypeString() {
        int oldHandle = passHandle;
        try {
            byte tc = bin.peekByte();
            switch (tc) {
                case TC_NULL:
                    return (String) readNull();
                case TC_REFERENCE:
                    return (String) readHandle(false);
                case TC_STRING:
                case TC_LONGSTRING:
                    return readString(false);
                default:
                    throw new Exception();
            }
        } finally {
            passHandle = oldHandle;
        }
    }
    private Object readNull() {
        if (bin.readByte() != TC_NULL) {
            throw new Exception();
        }
        passHandle = NULL_HANDLE;
        return null;
    }
    private Object readHandle(boolean unshared) {
        if (bin.readByte() != TC_REFERENCE) {
            throw new Exception();
        }
        passHandle = bin.readInt() - baseWireHandle;
        if (passHandle < 0 || passHandle >= handles.size()) {
            throw new Exception();
        }
        if (unshared) {
            // REMIND: what type of exception to throw here?
            throw new Exception(
                "cannot read back reference as unshared");
        }
        Object obj = handles.lookupObject(passHandle);
        if (obj == unsharedMarker) {
            // REMIND: what type of exception to throw here?
            throw new Exception(
                "cannot read back reference to unshared object");
        }
        return obj;
    }
    //private Class readClass(boolean unshared) {
    //    if (bin.readByte() != TC_CLASS) {
    //        throw new Exception();
    //    }
    //    ObjectStreamClass desc = readClassDesc(false);
    //    Class cl = desc.forClass();
    //    passHandle = handles.assign(unshared ? unsharedMarker : cl);
    //    Exception resolveEx = desc.getResolveException();
    //    if (resolveEx != null) {
    //        handles.markException(passHandle, resolveEx);
    //    }
    //    handles.finish(passHandle);
    //    return cl;
    //}
    //private ObjectStreamClass readClassDesc(boolean unshared)
    //{
    //    byte tc = bin.peekByte();
    //    switch (tc) {
    //        case TC_NULL:
    //            return (ObjectStreamClass) readNull();
    //        case TC_REFERENCE:
    //            return (ObjectStreamClass) readHandle(unshared);
    //        case TC_PROXYCLASSDESC:
    //            return readProxyDesc(unshared);
    //        case TC_CLASSDESC:
    //            return readNonProxyDesc(unshared);
    //        default:
    //            throw new StreamCorruptedException(
    //                String.format("invalid type code: %02X", tc));
    //    }
    //}
    //private boolean isCustomSubclass() {
    //    // Return true if this class is a custom subclass of ObjectInputStream
    //    return getClass().getClassLoader()
    //                != ObjectInputStream.class.getClassLoader();
    //}
    //private ObjectStreamClass readProxyDesc(boolean unshared)
    //{
    //    if (bin.readByte() != TC_PROXYCLASSDESC) {
    //        throw new Exception();
    //    }
    //    ObjectStreamClass desc = new ObjectStreamClass();
    //    int descHandle = handles.assign(unshared ? unsharedMarker : desc);
    //    passHandle = NULL_HANDLE;
    //    int numIfaces = bin.readInt();
    //    String[] ifaces = new String[numIfaces];
    //    for (int i = 0; i < numIfaces; i++) {
    //        ifaces[i] = bin.readUTF();
    //    }
    //    Class cl = null;
    //    Exception resolveEx = null;
    //    bin.setBlockDataMode(true);
    //    try {
    //        if ((cl = resolveProxyClass(ifaces)) == null) {
    //            resolveEx = new Exception("null class");
    //        } else if (!Proxy.isProxyClass(cl)) {
    //            throw new InvalidClassException("Not a proxy");
    //        } else {
    //            // ReflectUtil.checkProxyPackageAccess makes a test
    //            // equivalent to isCustomSubclass so there's no need
    //            // to condition this call to isCustomSubclass == true here.
    //            ReflectUtil.checkProxyPackageAccess(
    //                    getClass().getClassLoader(),
    //                    cl.getInterfaces());
    //        }
    //    } catch (Exception ex) {
    //        resolveEx = ex;
    //    }
    //    skipCustomData();
    //    desc.initProxy(cl, resolveEx, readClassDesc(false));
    //    handles.finish(descHandle);
    //    passHandle = descHandle;
    //    return desc;
    //}
    //private ObjectStreamClass readNonProxyDesc(boolean unshared)
    //{
    //    if (bin.readByte() != TC_CLASSDESC) {
    //        throw new Exception();
    //    }
    //    ObjectStreamClass desc = new ObjectStreamClass();
    //    int descHandle = handles.assign(unshared ? unsharedMarker : desc);
    //    passHandle = NULL_HANDLE;
    //    ObjectStreamClass readDesc = null;
    //    try {
    //        readDesc = readClassDescriptor();
    //    } catch (Exception ex) {
    //        throw (IOException) new InvalidClassException(
    //            "failed to read class descriptor").initCause(ex);
    //    }
    //    Class cl = null;
    //    Exception resolveEx = null;
    //    bin.setBlockDataMode(true);
    //    readonly boolean checksRequired = isCustomSubclass();
    //    try {
    //        if ((cl = resolveClass(readDesc)) == null) {
    //            resolveEx = new Exception("null class");
    //        } else if (checksRequired) {
    //            ReflectUtil.checkPackageAccess(cl);
    //        }
    //    } catch (Exception ex) {
    //        resolveEx = ex;
    //    }
    //    skipCustomData();
    //    desc.initNonProxy(readDesc, cl, resolveEx, readClassDesc(false));
    //    handles.finish(descHandle);
    //    passHandle = descHandle;
    //    return desc;
    //}
    private String readString(boolean unshared) {
        String str;
        byte tc = bin.readByte();
        switch (tc) {
            case TC_STRING:
                str = bin.readUTF();
                break;
            case TC_LONGSTRING:
                throw new NotImplementedException();
                //str = bin.readLongUTF();
                //break;
            default:
                throw new Exception();
        }
        passHandle = handles.assign(unshared ? unsharedMarker : str);
        handles.finish(passHandle);
        return str;
    }
    //private Object readArray(boolean unshared) {
    //    if (bin.readByte() != TC_ARRAY) {
    //        throw new Exception();
    //    }
    //    ObjectStreamClass desc = readClassDesc(false);
    //    int len = bin.readInt();
    //    Object array = null;
    //    Class cl, ccl = null;
    //    if ((cl = desc.forClass()) != null) {
    //        ccl = cl.getComponentType();
    //        array = Array.newInstance(ccl, len);
    //    }
    //    int arrayHandle = handles.assign(unshared ? unsharedMarker : array);
    //    Exception resolveEx = desc.getResolveException();
    //    if (resolveEx != null) {
    //        handles.markException(arrayHandle, resolveEx);
    //    }
    //    if (ccl == null) {
    //        for (int i = 0; i < len; i++) {
    //            readObject0(false);
    //        }
    //    } else if (ccl.isPrimitive()) {
    //        if (ccl == Integer.TYPE) {
    //            bin.readInts((int[]) array, 0, len);
    //        } else if (ccl == Byte.TYPE) {
    //            bin.readFully((byte[]) array, 0, len, true);
    //        } else if (ccl == Long.TYPE) {
    //            bin.readLongs((long[]) array, 0, len);
    //        } else if (ccl == Float.TYPE) {
    //            bin.readFloats((float[]) array, 0, len);
    //        } else if (ccl == Double.TYPE) {
    //            bin.readDoubles((double[]) array, 0, len);
    //        } else if (ccl == Short.TYPE) {
    //            bin.readShorts((short[]) array, 0, len);
    //        } else if (ccl == Character.TYPE) {
    //            bin.readChars((char[]) array, 0, len);
    //        } else if (ccl == Boolean.TYPE) {
    //            bin.readBooleans((boolean[]) array, 0, len);
    //        } else {
    //            throw new Exception();
    //        }
    //    } else {
    //        Object[] oa = (Object[]) array;
    //        for (int i = 0; i < len; i++) {
    //            oa[i] = readObject0(false);
    //            handles.markDependency(arrayHandle, passHandle);
    //        }
    //    }
    //    handles.finish(arrayHandle);
    //    passHandle = arrayHandle;
    //    return array;
    //}
    //private Enum readEnum(boolean unshared) {
    //    if (bin.readByte() != TC_ENUM) {
    //        throw new Exception();
    //    }
    //    ObjectStreamClass desc = readClassDesc(false);
    //    if (!desc.isEnum()) {
    //        throw new InvalidClassException("non-enum class: " + desc);
    //    }
    //    int enumHandle = handles.assign(unshared ? unsharedMarker : null);
    //    Exception resolveEx = desc.getResolveException();
    //    if (resolveEx != null) {
    //        handles.markException(enumHandle, resolveEx);
    //    }
    //    String name = readString(false);
    //    Enum en = null;
    //    Class cl = desc.forClass();
    //    if (cl != null) {
    //        try {
    //            en = Enum.valueOf(cl, name);
    //        } catch (IllegalArgumentException ex) {
    //            throw (IOException) new InvalidObjectException(
    //                "enum constant " + name + " does not exist @in " +
    //                cl).initCause(ex);
    //        }
    //        if (!unshared) {
    //            handles.setObject(enumHandle, en);
    //        }
    //    }
    //    handles.finish(enumHandle);
    //    passHandle = enumHandle;
    //    return en;
    //}
    //private Object readOrdinaryObject(boolean unshared)
    //{
    //    if (bin.readByte() != TC_OBJECT) {
    //        throw new Exception();
    //    }
    //    ObjectStreamClass desc = readClassDesc(false);
    //    desc.checkDeserialize();
    //    Type cl = desc.forClass();
    //    if (cl == String.class || cl == Class.class
    //            || cl == ObjectStreamClass.class) {
    //        throw new InvalidClassException("invalid class descriptor");
    //    }
    //    Object obj;
    //    try {
    //        obj = desc.isInstantiable() ? desc.newInstance() : null;
    //    } catch (Exception ex) {
    //        throw (IOException) new InvalidClassException(
    //            desc.forClass().getName(),
    //            "unable to create instance").initCause(ex);
    //    }
    //    passHandle = handles.assign(unshared ? unsharedMarker : obj);
    //    Exception resolveEx = desc.getResolveException();
    //    if (resolveEx != null) {
    //        handles.markException(passHandle, resolveEx);
    //    }
    //    if (desc.isExternalizable()) {
    //        readExternalData((Externalizable) obj, desc);
    //    } else {
    //        readSerialData(obj, desc);
    //    }
    //    handles.finish(passHandle);
    //    if (obj != null &&
    //        handles.lookupException(passHandle) == null &&
    //        desc.hasReadResolveMethod())
    //    {
    //        Object rep = desc.invokeReadResolve(obj);
    //        if (unshared && rep.getClass().isArray()) {
    //            rep = cloneArray(rep);
    //        }
    //        if (rep != obj) {
    //            handles.setObject(passHandle, obj = rep);
    //        }
    //    }
    //    return obj;
    //}
    //private void readExternalData(Externalizable obj, ObjectStreamClass desc)
    //{
    //    SerialCallbackContext oldContext = curContext;
    //    curContext = null;
    //    try {
    //        boolean blocked = desc.hasBlockExternalData();
    //        if (blocked) {
    //            bin.setBlockDataMode(true);
    //        }
    //        if (obj != null) {
    //            try {
    //                obj.readExternal(this);
    //            } catch (Exception ex) {
    //                /*
    //                 * @in most cases, the handle table has already propagated
    //                 * a CNFException to passHandle at this point; this mark
    //                 * call is included to address cases where the readExternal
    //                 * method has cons'ed and thrown a new CNFException of its
    //                 * own.
    //                 */
    //                 handles.markException(passHandle, ex);
    //            }
    //        }
    //        if (blocked) {
    //            skipCustomData();
    //        }
    //    } finally {
    //        curContext = oldContext;
    //    }
    //    /*
    //     * At this point, if the externalizable data was not written @in
    //     * block-data form and either the externalizable class doesn't exist
    //     * locally (i.e., obj == null) or readExternal() just threw a
    //     * CNFException, then the stream is probably @in an inconsistent state,
    //     * since some (or all) of the externalizable data may not have been
    //     * consumed.  Since there's no "correct" action to take @in this case,
    //     * we mimic the behavior of past serialization implementations and
    //     * blindly hope that the stream is @in sync; if it isn't and additional
    //     * externalizable data remains @in the stream, a subsequent read will
    //     * most likely throw a StreamCorruptedException.
    //     */
    //}
    //private void readSerialData(Object obj, ObjectStreamClass desc)
    //{
    //    ObjectStreamClass.ClassDataSlot[] slots = desc.getClassDataLayout();
    //    for (int i = 0; i < slots.length; i++) {
    //        ObjectStreamClass slotDesc = slots[i].desc;
    //        if (slots[i].hasData) {
    //            if (obj != null &&
    //                slotDesc.hasReadObjectMethod() &&
    //                handles.lookupException(passHandle) == null)
    //            {
    //                SerialCallbackContext oldContext = curContext;
    //                try {
    //                    curContext = new SerialCallbackContext(obj, slotDesc);
    //                    bin.setBlockDataMode(true);
    //                    slotDesc.invokeReadObject(obj, this);
    //                } catch (Exception ex) {
    //                    /*
    //                     * @in most cases, the handle table has already
    //                     * propagated a CNFException to passHandle at this
    //                     * point; this mark call is included to address cases
    //                     * where the custom readObject method has cons'ed and
    //                     * thrown a new CNFException of its own.
    //                     */
    //                    handles.markException(passHandle, ex);
    //                } finally {
    //                    curContext.setUsed();
    //                    curContext = oldContext;
    //                }
    //                /*
    //                 * defaultDataEnd may have been set indirectly by custom
    //                 * readObject() method when calling defaultReadObject() or
    //                 * readFields(); clear it to restore normal read behavior.
    //                 */
    //                defaultDataEnd = false;
    //            } else {
    //                defaultReadFields(obj, slotDesc);
    //            }
    //            if (slotDesc.hasWriteObjectData()) {
    //                skipCustomData();
    //            } else {
    //                bin.setBlockDataMode(false);
    //            }
    //        } else {
    //            if (obj != null &&
    //                slotDesc.hasReadObjectNoDataMethod() &&
    //                handles.lookupException(passHandle) == null)
    //            {
    //                slotDesc.invokeReadObjectNoData(obj);
    //            }
    //        }
    //    }
    //}
    //private void skipCustomData() {
    //    int oldHandle = passHandle;
    //    for (;;) {
    //        if (bin.getBlockDataMode()) {
    //            bin.skipBlockData();
    //            bin.setBlockDataMode(false);
    //        }
    //        switch (bin.peekByte()) {
    //            case TC_BLOCKDATA:
    //            case TC_BLOCKDATALONG:
    //                bin.setBlockDataMode(true);
    //                break;
    //            case TC_ENDBLOCKDATA:
    //                bin.readByte();
    //                passHandle = oldHandle;
    //                return;
    //            default:
    //                readObject0(false);
    //                break;
    //        }
    //    }
    //}
    //private void defaultReadFields(Object obj, ObjectStreamClass desc)
    //{
    //    // REMIND: is isInstance check necessary?
    //    Class cl = desc.forClass();
    //    if (cl != null && obj != null && !cl.isInstance(obj)) {
    //        throw new ClassCastException();
    //    }
    //    int primDataSize = desc.getPrimDataSize();
    //    if (primVals == null || primVals.length < primDataSize) {
    //        primVals = new byte[primDataSize];
    //    }
    //    bin.readFully(primVals, 0, primDataSize, false);
    //    if (obj != null) {
    //        desc.setPrimFieldValues(obj, primVals);
    //    }
    //    int objHandle = passHandle;
    //    ObjectStreamField[] fields = desc.getFields(false);
    //    Object[] objVals = new Object[desc.getNumObjFields()];
    //    int numPrimFields = fields.length - objVals.length;
    //    for (int i = 0; i < objVals.length; i++) {
    //        ObjectStreamField f = fields[numPrimFields + i];
    //        objVals[i] = readObject0(f.isUnshared());
    //        if (f.getField() != null) {
    //            handles.markDependency(objHandle, passHandle);
    //        }
    //    }
    //    if (obj != null) {
    //        desc.setObjFieldValues(obj, objVals);
    //    }
    //    passHandle = objHandle;
    //}
    //private IOException readFatalException() {
    //    if (bin.readByte() != TC_EXCEPTION) {
    //        throw new Exception();
    //    }
    //    clear();
    //    return (IOException) readObject0(false);
    //}
    private void handleReset() {
        if (depth > 0) {
            throw new Exception(
                "unexpected reset; recursion depth: " + depth);
        }
        clear();
    }
    //// REMIND: remove once hotspot inlines Float.intBitsToFloat
    //private static native void bytesToFloats(byte[] src, int srcpos,
    //                                         float[] dst, int dstpos,
    //                                         int nfloats);
    //// REMIND: remove once hotspot inlines Double.longBitsToDouble
    //private static native void bytesToDoubles(byte[] src, int srcpos,
    //                                          double[] dst, int dstpos,
    //                                          int ndoubles);
    //// REMIND: change name to something more accurate?
    //private static native ClassLoader latestUserDefinedLoader();
    //private class GetFieldImpl extends GetField {
    //    private readonly ObjectStreamClass desc;
    //    private readonly byte[] primVals;
    //    private readonly Object[] objVals;
    //    private readonly int[] objHandles;
    //    GetFieldImpl(ObjectStreamClass desc) {
    //        this.desc = desc;
    //        primVals = new byte[desc.getPrimDataSize()];
    //        objVals = new Object[desc.getNumObjFields()];
    //        objHandles = new int[objVals.length];
    //    }
    //    public ObjectStreamClass getObjectStreamClass() {
    //        return desc;
    //    }
    //    public boolean defaulted(String name) {
    //        return (getFieldOffset(name, null) < 0);
    //    }
    //    public boolean get(String name, boolean val) {
    //        int off = getFieldOffset(name, Boolean.TYPE);
    //        return (off >= 0) ? Bits.getBoolean(primVals, off) : val;
    //    }
    //    public byte get(String name, byte val) {
    //        int off = getFieldOffset(name, Byte.TYPE);
    //        return (off >= 0) ? primVals[off] : val;
    //    }
    //    public char get(String name, char val) {
    //        int off = getFieldOffset(name, Character.TYPE);
    //        return (off >= 0) ? Bits.getChar(primVals, off) : val;
    //    }
    //    public short get(String name, short val) {
    //        int off = getFieldOffset(name, Short.TYPE);
    //        return (off >= 0) ? Bits.getShort(primVals, off) : val;
    //    }
    //    public int get(String name, int val) {
    //        int off = getFieldOffset(name, Integer.TYPE);
    //        return (off >= 0) ? Bits.getInt(primVals, off) : val;
    //    }
    //    public float get(String name, float val) {
    //        int off = getFieldOffset(name, Float.TYPE);
    //        return (off >= 0) ? Bits.getFloat(primVals, off) : val;
    //    }
    //    public long get(String name, long val) {
    //        int off = getFieldOffset(name, Long.TYPE);
    //        return (off >= 0) ? Bits.getLong(primVals, off) : val;
    //    }
    //    public double get(String name, double val) {
    //        int off = getFieldOffset(name, Double.TYPE);
    //        return (off >= 0) ? Bits.getDouble(primVals, off) : val;
    //    }
    //    public Object get(String name, Object val) {
    //        int off = getFieldOffset(name, Object.class);
    //        if (off >= 0) {
    //            int objHandle = objHandles[off];
    //            handles.markDependency(passHandle, objHandle);
    //            return (handles.lookupException(objHandle) == null) ?
    //                objVals[off] : null;
    //        } else {
    //            return val;
    //        }
    //    }
    //    void readFields() {
    //        bin.readFully(primVals, 0, primVals.length, false);
    //        int oldHandle = passHandle;
    //        ObjectStreamField[] fields = desc.getFields(false);
    //        int numPrimFields = fields.length - objVals.length;
    //        for (int i = 0; i < objVals.length; i++) {
    //            objVals[i] =
    //                readObject0(fields[numPrimFields + i].isUnshared());
    //            objHandles[i] = passHandle;
    //        }
    //        passHandle = oldHandle;
    //    }
    //    private int getFieldOffset(String name, Class type) {
    //        ObjectStreamField field = desc.getField(name, type);
    //        if (field != null) {
    //            return field.getOffset();
    //        } else if (desc.getLocalDesc().getField(name, type) != null) {
    //            return -1;
    //        } else {
    //            throw new IllegalArgumentException("no such field " + name +
    //                                               " with type " + type);
    //        }
    //    }
    //}
    //private static class ValidationList {
    //    private static class Callback {
    //        readonly ObjectInputValidation obj;
    //        readonly int priority;
    //        Callback next;
    //        readonly AccessControlContext acc;
    //        Callback(ObjectInputValidation obj, int priority, Callback next,
    //            AccessControlContext acc)
    //        {
    //            this.obj = obj;
    //            this.priority = priority;
    //            this.next = next;
    //            this.acc = acc;
    //        }
    //    }
    //    private Callback list;
    //    ValidationList() {
    //    }
    //    void register(ObjectInputValidation obj, int priority)
    //    {
    //        if (obj == null) {
    //            throw new InvalidObjectException("null callback");
    //        }
    //        Callback prev = null, cur = list;
    //        while (cur != null && priority < cur.priority) {
    //            prev = cur;
    //            cur = cur.next;
    //        }
    //        AccessControlContext acc = AccessController.getContext();
    //        if (prev != null) {
    //            prev.next = new Callback(obj, priority, cur, acc);
    //        } else {
    //            list = new Callback(obj, priority, list, acc);
    //        }
    //    }
    //    void doCallbacks() {
    //        try {
    //            while (list != null) {
    //                AccessController.doPrivileged(
    //                    new PrivilegedExceptionAction<Void>()
    //                {
    //                    public Void run() {
    //                        list.obj.validateObject();
    //                        return null;
    //                    }
    //                }, list.acc);
    //                list = list.next;
    //            }
    //        } catch (PrivilegedActionException ex) {
    //            list = null;
    //            throw (InvalidObjectException) ex.getException();
    //        }
    //    }
    //    public void clear() {
    //        list = null;
    //    }
    //}
    private class PeekInputStream : InputStream {
        private readonly InputStream @in;
        private int peekb = -1;
        internal PeekInputStream(InputStream @in) {
            this.@in = @in;
        }
        internal int peek() {
            return (peekb >= 0) ? peekb : (peekb = @in.read());
        }
        public override int read() {
            if (peekb >= 0) {
                int v = peekb;
                peekb = -1;
                return v;
            } else {
                return @in.read();
            }
        }
        public override int read(byte[] b, int off, int len) {
            if (len == 0) {
                return 0;
            } else if (peekb < 0) {
                return @in.read(b, off, len);
            } else {
                b[off++] = (byte) peekb;
                len--;
                peekb = -1;
                int n = @in.read(b, off, len);
                return (n >= 0) ? (n + 1) : 1;
            }
        }
        internal void readFully(byte[] b, int off, int len) {
            int n = 0;
            while (n < len) {
                int count = read(b, off + n, len - n);
                if (count < 0) {
                    throw new Exception();
                }
                n += count;
            }
        }
        public override long skip(long n) {
            if (n <= 0) {
                return 0;
            }
            int skipped = 0;
            if (peekb >= 0) {
                peekb = -1;
                skipped++;
                n--;
            }
            return skipped + skip(n);
        }
        public override int available() {
            return @in.available() + ((peekb >= 0) ? 1 : 0);
        }
        public override void close() {
            @in.close();
        }
    }
    private class BlockDataInputStream
        : InputStream, DataInput
    {
        private const int MAX_BLOCK_SIZE = 1024;
        private const int MAX_HEADER_SIZE = 5;
        private const int CHAR_BUF_SIZE = 256;
        private const int HEADER_BLOCKED = -2;
        private readonly byte[] buf = new byte[MAX_BLOCK_SIZE];
        private readonly byte[] hbuf = new byte[MAX_HEADER_SIZE];
        private readonly char[] cbuf = new char[CHAR_BUF_SIZE];
        private boolean blkmode = false;
        // block data state fields; values meaningful only when blkmode true
        private int pos = 0;
        private int end = -1;
        private int unread = 0;
        private readonly PeekInputStream @in;
        private readonly DataInputStream din;
        internal BlockDataInputStream(InputStream @in) {
            this.@in = new PeekInputStream(@in);
            din = new DataInputStream(this);
        }
        internal boolean setBlockDataMode(boolean newmode) {
            if (blkmode == newmode) {
                return blkmode;
            }
            if (newmode) {
                pos = 0;
                end = 0;
                unread = 0;
            } else if (pos < end) {
                throw new IllegalStateException("unread block data");
            }
            blkmode = newmode;
            return !blkmode;
        }
        internal boolean getBlockDataMode() {
            return blkmode;
        }
        internal void skipBlockData() {
            if (!blkmode) {
                throw new IllegalStateException("not @in block data mode");
            }
            while (end >= 0) {
                refill();
            }
        }
        private int readBlockHeader(boolean canBlock) {
            //if (defaultDataEnd) {
            //    /*
            //     * Fix for 4360508: stream is currently at the end of a field
            //     * value block written via default serialization; since there
            //     * is no terminating TC_ENDBLOCKDATA tag, simulate
            //     * end-of-custom-data behavior explicitly.
            //     */
            //    return -1;
            //}
            try {
                for (;;) {
                    int avail = canBlock ? Integer.MAX_VALUE : @in.available();
                    if (avail == 0) {
                        return HEADER_BLOCKED;
                    }
                    int tc = @in.peek();
                    switch (tc) {
                        case TC_BLOCKDATA:
                            if (avail < 2) {
                                return HEADER_BLOCKED;
                            }
                            @in.readFully(hbuf, 0, 2);
                            return hbuf[1] & 0xFF;
                        case TC_BLOCKDATALONG:
                            if (avail < 5) {
                                return HEADER_BLOCKED;
                            }
                            @in.readFully(hbuf, 0, 5);
                            int len = Bits.getInt(hbuf, 1);
                            if (len < 0) {
                                throw new Exception(
                                    "illegal block data header length: " +
                                    len);
                            }
                            return len;
                        /*
                         * TC_RESETs may occur @in between data blocks.
                         * Unfortunately, this case must be parsed at a lower
                         * level than other typecodes, since primitive data
                         * reads may span data blocks separated by a TC_RESET.
                         */
                        case TC_RESET:
                            throw new NotImplementedException();
                            //@in.read();
                            //handleReset();
                            //break;
                        default:
                            if (tc >= 0 && (tc < TC_BASE || tc > TC_MAX)) {
                                throw new Exception();
                            }
                            return -1;
                    }
                }
            } catch (Exception) {
                throw new Exception(
                    "unexpected EOF while reading block data header");
            }
        }
        private void refill() {
            try {
                do {
                    pos = 0;
                    if (unread > 0) {
                        int n =
                            @in.read(buf, 0, Math.min(unread, MAX_BLOCK_SIZE));
                        if (n >= 0) {
                            end = n;
                            unread -= n;
                        } else {
                            throw new Exception(
                                "unexpected EOF @in middle of data block");
                        }
                    } else {
                        int n = readBlockHeader(true);
                        if (n >= 0) {
                            end = 0;
                            unread = n;
                        } else {
                            end = -1;
                            unread = 0;
                        }
                    }
                } while (pos == end);
            } catch (IOException ex) {
                pos = 0;
                end = -1;
                unread = 0;
                throw ex;
            }
        }
        internal int currentBlockRemaining() {
            if (blkmode) {
                return (end >= 0) ? (end - pos) + unread : 0;
            } else {
                throw new Exception();
            }
        }
        int peek() {
            if (blkmode) {
                if (pos == end) {
                    refill();
                }
                return (end >= 0) ? (buf[pos] & 0xFF) : -1;
            } else {
                return @in.peek();
            }
        }
        internal byte peekByte() {
            int val = peek();
            if (val < 0) {
                throw new Exception();
            }
            return (byte) val;
        }
        /* ----------------- generic input stream methods ------------------ */
        /*
         * The following methods are equivalent to their counterparts @in
         * InputStream, except that they interpret data block boundaries and
         * read the requested data from within data blocks when @in block data
         * mode.
         */
        public override int read() {
            if (blkmode) {
                if (pos == end) {
                    refill();
                }
                return (end >= 0) ? (buf[pos++] & 0xFF) : -1;
            } else {
                return @in.read();
            }
        }
        public override int read(byte[] b, int off, int len) {
            return read(b, off, len, false);
        }
        public override long skip(long len) {
            long remain = len;
            while (remain > 0) {
                if (blkmode) {
                    if (pos == end) {
                        refill();
                    }
                    if (end < 0) {
                        break;
                    }
                    int nread = (int) Math.min(remain, end - pos);
                    remain -= nread;
                    pos += nread;
                } else {
                    int nread = (int) Math.min(remain, MAX_BLOCK_SIZE);
                    if ((nread = @in.read(buf, 0, nread)) < 0) {
                        break;
                    }
                    remain -= nread;
                }
            }
            return len - remain;
        }
        public override int available() {
            if (blkmode) {
                if ((pos == end) && (unread == 0)) {
                    int n;
                    while ((n = readBlockHeader(false)) == 0) ;
                    switch (n) {
                        case HEADER_BLOCKED:
                            break;
                        case -1:
                            pos = 0;
                            end = -1;
                            break;
                        default:
                            pos = 0;
                            end = 0;
                            unread = n;
                            break;
                    }
                }
                // avoid unnecessary call to @in.available() if possible
                int unreadAvail = (unread > 0) ?
                    Math.min(@in.available(), unread) : 0;
                return (end >= 0) ? (end - pos) + unreadAvail : 0;
            } else {
                return @in.available();
            }
        }
        public override void close() {
            if (blkmode) {
                pos = 0;
                end = -1;
                unread = 0;
            }
            @in.close();
        }
        internal int read(byte[] b, int off, int len, boolean copy) {
            if (len == 0) {
                return 0;
            } else if (blkmode) {
                if (pos == end) {
                    refill();
                }
                if (end < 0) {
                    return -1;
                }
                int nread = Math.min(len, end - pos);
                System.Array.Copy(buf, pos, b, off, nread);
                pos += nread;
                return nread;
            } else if (copy) {
                int nread = @in.read(buf, 0, Math.min(len, MAX_BLOCK_SIZE));
                if (nread > 0) {
                    System.Array.Copy(buf, 0, b, off, nread);
                }
                return nread;
            } else {
                return @in.read(b, off, len);
            }
        }
        /* ----------------- primitive data input methods ------------------ */
        /*
         * The following methods are equivalent to their counterparts @in
         * DataInputStream, except that they interpret data block boundaries
         * and read the requested data from within data blocks when @in block
         * data mode.
         */
        public void readFully(byte[] b) {
            readFully(b, 0, b.Length, false);
        }
        public void readFully(byte[] b, int off, int len) {
            readFully(b, off, len, false);
        }
        public void readFully(byte[] b, int off, int len, boolean copy)
        {
            while (len > 0) {
                int n = read(b, off, len, copy);
                if (n < 0) {
                    throw new Exception();
                }
                off += n;
                len -= n;
            }
        }
        public int skipBytes(int n) {
            return din.skipBytes(n);
        }
        public boolean readBoolean() {
            int v = read();
            if (v < 0) {
                throw new Exception();
            }
            return (v != 0);
        }
        public byte readByte() {
            int v = read();
            if (v < 0) {
                throw new Exception();
            }
            return (byte) v;
        }
        public int readUnsignedByte() {
            int v = read();
            if (v < 0) {
                throw new Exception();
            }
            return v;
        }
        public char readChar() {
            if (!blkmode) {
                pos = 0;
                @in.readFully(buf, 0, 2);
            } else if (end - pos < 2) {
                return din.readChar();
            }
            char v = Bits.getChar(buf, pos);
            pos += 2;
            return v;
        }
        public short readShort() {
            if (!blkmode) {
                pos = 0;
                @in.readFully(buf, 0, 2);
            } else if (end - pos < 2) {
                return din.readShort();
            }
            short v = Bits.getShort(buf, pos);
            pos += 2;
            return v;
        }
        public int readUnsignedShort() {
            if (!blkmode) {
                pos = 0;
                @in.readFully(buf, 0, 2);
            } else if (end - pos < 2) {
                return din.readUnsignedShort();
            }
            int v = Bits.getShort(buf, pos) & 0xFFFF;
            pos += 2;
            return v;
        }
        public int readInt() {
            if (!blkmode) {
                pos = 0;
                @in.readFully(buf, 0, 4);
            } else if (end - pos < 4) {
                return din.readInt();
            }
            int v = Bits.getInt(buf, pos);
            pos += 4;
            return v;
        }
        //public float readFloat() {
        //    if (!blkmode) {
        //        pos = 0;
        //        @in.readFully(buf, 0, 4);
        //    } else if (end - pos < 4) {
        //        return din.readFloat();
        //    }
        //    float v = Bits.getFloat(buf, pos);
        //    pos += 4;
        //    return v;
        //}
        //public long readLong() {
        //    if (!blkmode) {
        //        pos = 0;
        //        @in.readFully(buf, 0, 8);
        //    } else if (end - pos < 8) {
        //        return din.readLong();
        //    }
        //    long v = Bits.getLong(buf, pos);
        //    pos += 8;
        //    return v;
        //}
        //public double readDouble() {
        //    if (!blkmode) {
        //        pos = 0;
        //        @in.readFully(buf, 0, 8);
        //    } else if (end - pos < 8) {
        //        return din.readDouble();
        //    }
        //    double v = Bits.getDouble(buf, pos);
        //    pos += 8;
        //    return v;
        //}
        public String readUTF() {
            return readUTFBody(readUnsignedShort());
        }
        //public String readLine() {
        //    return din.readLine();      // deprecated, not worth optimizing
        //}
        /* -------------- primitive data array input methods --------------- */
        /*
         * The following methods read @in spans of primitive data values.
         * Though equivalent to calling the corresponding primitive read
         * methods repeatedly, these methods are optimized for reading groups
         * of primitive data values more efficiently.
         */
        void readBooleans(boolean[] v, int off, int len) {
            int stop, endoff = off + len;
            while (off < endoff) {
                if (!blkmode) {
                    int span = Math.min(endoff - off, MAX_BLOCK_SIZE);
                    @in.readFully(buf, 0, span);
                    stop = off + span;
                    pos = 0;
                } else if (end - pos < 1) {
                    v[off++] = din.readBoolean();
                    continue;
                } else {
                    stop = Math.min(endoff, off + end - pos);
                }
                while (off < stop) {
                    v[off++] = Bits.getBoolean(buf, pos++);
                }
            }
        }
        void readChars(char[] v, int off, int len) {
            int stop, endoff = off + len;
            while (off < endoff) {
                if (!blkmode) {
                    int span = Math.min(endoff - off, MAX_BLOCK_SIZE >> 1);
                    @in.readFully(buf, 0, span << 1);
                    stop = off + span;
                    pos = 0;
                } else if (end - pos < 2) {
                    v[off++] = din.readChar();
                    continue;
                } else {
                    stop = Math.min(endoff, off + ((end - pos) >> 1));
                }
                while (off < stop) {
                    v[off++] = Bits.getChar(buf, pos);
                    pos += 2;
                }
            }
        }
        void readShorts(short[] v, int off, int len) {
            int stop, endoff = off + len;
            while (off < endoff) {
                if (!blkmode) {
                    int span = Math.min(endoff - off, MAX_BLOCK_SIZE >> 1);
                    @in.readFully(buf, 0, span << 1);
                    stop = off + span;
                    pos = 0;
                } else if (end - pos < 2) {
                    v[off++] = din.readShort();
                    continue;
                } else {
                    stop = Math.min(endoff, off + ((end - pos) >> 1));
                }
                while (off < stop) {
                    v[off++] = Bits.getShort(buf, pos);
                    pos += 2;
                }
            }
        }
        void readInts(int[] v, int off, int len) {
            int stop, endoff = off + len;
            while (off < endoff) {
                if (!blkmode) {
                    int span = Math.min(endoff - off, MAX_BLOCK_SIZE >> 2);
                    @in.readFully(buf, 0, span << 2);
                    stop = off + span;
                    pos = 0;
                } else if (end - pos < 4) {
                    v[off++] = din.readInt();
                    continue;
                } else {
                    stop = Math.min(endoff, off + ((end - pos) >> 2));
                }
                while (off < stop) {
                    v[off++] = Bits.getInt(buf, pos);
                    pos += 4;
                }
            }
        }
        //void readFloats(float[] v, int off, int len) {
        //    int span, endoff = off + len;
        //    while (off < endoff) {
        //        if (!blkmode) {
        //            span = Math.min(endoff - off, MAX_BLOCK_SIZE >> 2);
        //            @in.readFully(buf, 0, span << 2);
        //            pos = 0;
        //        } else if (end - pos < 4) {
        //            v[off++] = din.readFloat();
        //            continue;
        //        } else {
        //            span = Math.min(endoff - off, ((end - pos) >> 2));
        //        }
        //        bytesToFloats(buf, pos, v, off, span);
        //        off += span;
        //        pos += span << 2;
        //    }
        //}
        //void readLongs(long[] v, int off, int len) {
        //    int stop, endoff = off + len;
        //    while (off < endoff) {
        //        if (!blkmode) {
        //            int span = Math.min(endoff - off, MAX_BLOCK_SIZE >> 3);
        //            @in.readFully(buf, 0, span << 3);
        //            stop = off + span;
        //            pos = 0;
        //        } else if (end - pos < 8) {
        //            v[off++] = din.readLong();
        //            continue;
        //        } else {
        //            stop = Math.min(endoff, off + ((end - pos) >> 3));
        //        }
        //        while (off < stop) {
        //            v[off++] = Bits.getLong(buf, pos);
        //            pos += 8;
        //        }
        //    }
        //}
        //void readDoubles(double[] v, int off, int len) {
        //    int span, endoff = off + len;
        //    while (off < endoff) {
        //        if (!blkmode) {
        //            span = Math.min(endoff - off, MAX_BLOCK_SIZE >> 3);
        //            @in.readFully(buf, 0, span << 3);
        //            pos = 0;
        //        } else if (end - pos < 8) {
        //            v[off++] = din.readDouble();
        //            continue;
        //        } else {
        //            span = Math.min(endoff - off, ((end - pos) >> 3));
        //        }
        //        bytesToDoubles(buf, pos, v, off, span);
        //        off += span;
        //        pos += span << 3;
        //    }
        //}
        //String readLongUTF() {
        //    return readUTFBody(readLong());
        //}
        private String readUTFBody(long utflen) {
            StringBuilder sbuf = new StringBuilder();
            if (!blkmode) {
                end = pos = 0;
            }
            while (utflen > 0) {
                int avail = end - pos;
                if (avail >= 3 || (long) avail == utflen) {
                    utflen -= readUTFSpan(sbuf, utflen);
                } else {
                    if (blkmode) {
                        // near block boundary, read one byte at a time
                        utflen -= readUTFChar(sbuf, utflen);
                    } else {
                        // shift and refill buffer manually
                        if (avail > 0) {
                            System.Array.Copy(buf, pos, buf, 0, avail);
                        }
                        pos = 0;
                        end = (int) Math.min(MAX_BLOCK_SIZE, (int)utflen);
                        @in.readFully(buf, avail, end - avail);
                    }
                }
            }
            return sbuf.toString();
        }
        private long readUTFSpan(StringBuilder sbuf, long utflen)
        {
            int cpos = 0;
            int start = pos;
            int avail = Math.min(end - pos, CHAR_BUF_SIZE);
            // stop short of last char unless all of utf bytes @in buffer
            int stop = pos + ((utflen > avail) ? avail - 2 : (int) utflen);
            boolean outOfBounds = false;
            try {
                while (pos < stop) {
                    int b1, b2, b3;
                    b1 = buf[pos++] & 0xFF;
                    switch (b1 >> 4) {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:   // 1 byte format: 0xxxxxxx
                            cbuf[cpos++] = (char) b1;
                            break;
                        case 12:
                        case 13:  // 2 byte format: 110xxxxx 10xxxxxx
                            b2 = buf[pos++];
                            if ((b2 & 0xC0) != 0x80) {
                                throw new Exception();
                            }
                            cbuf[cpos++] = (char) (((b1 & 0x1F) << 6) |
                                                   ((b2 & 0x3F) << 0));
                            break;
                        case 14:  // 3 byte format: 1110xxxx 10xxxxxx 10xxxxxx
                            b3 = buf[pos + 1];
                            b2 = buf[pos + 0];
                            pos += 2;
                            if ((b2 & 0xC0) != 0x80 || (b3 & 0xC0) != 0x80) {
                                throw new Exception();
                            }
                            cbuf[cpos++] = (char) (((b1 & 0x0F) << 12) |
                                                   ((b2 & 0x3F) << 6) |
                                                   ((b3 & 0x3F) << 0));
                            break;
                        default:  // 10xx xxxx, 1111 xxxx
                            throw new Exception();
                    }
                }
            } catch (Exception) {
                outOfBounds = true;
            } finally {
                if (outOfBounds || (pos - start) > utflen) {
                    /*
                     * Fix for 4450867: if a malformed utf char causes the
                     * conversion loop to scan past the expected end of the utf
                     * string, only consume the expected number of utf bytes.
                     */
                    pos = start + (int) utflen;
                    throw new Exception();
                }
            }
            sbuf.append(cbuf, 0, cpos);
            return pos - start;
        }
        private int readUTFChar(StringBuilder sbuf, long utflen)
        {
            int b1, b2, b3;
            b1 = readByte() & 0xFF;
            switch (b1 >> 4) {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:     // 1 byte format: 0xxxxxxx
                    sbuf.append((char) b1);
                    return 1;
                case 12:
                case 13:    // 2 byte format: 110xxxxx 10xxxxxx
                    if (utflen < 2) {
                        throw new Exception();
                    }
                    b2 = readByte();
                    if ((b2 & 0xC0) != 0x80) {
                        throw new Exception();
                    }
                    sbuf.append((char) (((b1 & 0x1F) << 6) |
                                        ((b2 & 0x3F) << 0)));
                    return 2;
                case 14:    // 3 byte format: 1110xxxx 10xxxxxx 10xxxxxx
                    if (utflen < 3) {
                        if (utflen == 2) {
                            readByte();         // consume remaining byte
                        }
                        throw new Exception();
                    }
                    b2 = readByte();
                    b3 = readByte();
                    if ((b2 & 0xC0) != 0x80 || (b3 & 0xC0) != 0x80) {
                        throw new Exception();
                    }
                    sbuf.append((char) (((b1 & 0x0F) << 12) |
                                        ((b2 & 0x3F) << 6) |
                                        ((b3 & 0x3F) << 0)));
                    return 3;
                default:   // 10xx xxxx, 1111 xxxx
                    throw new Exception();
            }
        }
    }
    // REMIND: add full description of exception propagation algorithm?
    private class HandleTable {
        /* status codes indicating whether object has associated exception */
        private const byte STATUS_OK = 1;
        private const byte STATUS_UNKNOWN = 2;
        private const byte STATUS_EXCEPTION = 3;
        byte[] status;
        Object[] entries;
        HandleList[] deps;
        int lowDep = -1;
        int _size = 0;
        internal HandleTable(int initialCapacity) {
            status = new byte[initialCapacity];
            entries = new Object[initialCapacity];
            deps = new HandleList[initialCapacity];
        }
        internal int assign(Object obj) {
            if (_size >= entries.Length) {
                grow();
            }
            status[_size] = STATUS_UNKNOWN;
            entries[_size] = obj;
            return _size++;
        }
        internal void markDependency(int dependent, int target) {
            if (dependent == NULL_HANDLE || target == NULL_HANDLE) {
                return;
            }
            switch (status[dependent]) {
                case STATUS_UNKNOWN:
                    switch (status[target]) {
                        case STATUS_OK:
                            // ignore dependencies on objs with no exception
                            break;
                        case STATUS_EXCEPTION:
                            // eagerly propagate exception
                            markException(dependent,
                                (Exception) entries[target]);
                            break;
                        case STATUS_UNKNOWN:
                            // add to dependency list of target
                            if (deps[target] == null) {
                                deps[target] = new HandleList();
                            }
                            deps[target].add(dependent);
                            // remember lowest unresolved target seen
                            if (lowDep < 0 || lowDep > target) {
                                lowDep = target;
                            }
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case STATUS_EXCEPTION:
                    break;
                default:
                    throw new Exception();
            }
        }
        internal void markException(int handle, Exception ex) {
            switch (status[handle]) {
                case STATUS_UNKNOWN:
                    status[handle] = STATUS_EXCEPTION;
                    entries[handle] = ex;
                    // propagate exception to dependents
                    HandleList dlist = deps[handle];
                    if (dlist != null) {
                        int ndeps = dlist.size();
                        for (int i = 0; i < ndeps; i++) {
                            markException(dlist.get(i), ex);
                        }
                        deps[handle] = null;
                    }
                    break;
                case STATUS_EXCEPTION:
                    break;
                default:
                    throw new Exception();
            }
        }
        internal void finish(int handle) {
            int end;
            if (lowDep < 0) {
                // no pending unknowns, only resolve current handle
                end = handle + 1;
            } else if (lowDep >= handle) {
                // pending unknowns now clearable, resolve all upward handles
                end = _size;
                lowDep = -1;
            } else {
                // unresolved backrefs present, can't resolve anything yet
                return;
            }
            // change STATUS_UNKNOWN -> STATUS_OK @in selected span of handles
            for (int i = handle; i < end; i++) {
                switch (status[i]) {
                    case STATUS_UNKNOWN:
                        status[i] = STATUS_OK;
                        deps[i] = null;
                        break;
                    case STATUS_OK:
                    case STATUS_EXCEPTION:
                        break;
                    default:
                        throw new Exception();
                }
            }
        }
        internal void setObject(int handle, Object obj) {
            switch (status[handle]) {
                case STATUS_UNKNOWN:
                case STATUS_OK:
                    entries[handle] = obj;
                    break;
                case STATUS_EXCEPTION:
                    break;
                default:
                    throw new Exception();
            }
        }
        internal Object lookupObject(int handle) {
            return (handle != NULL_HANDLE &&
                    status[handle] != STATUS_EXCEPTION) ?
                entries[handle] : null;
        }
        internal Exception lookupException(int handle) {
            return (handle != NULL_HANDLE &&
                    status[handle] == STATUS_EXCEPTION) ?
                (Exception) entries[handle] : null;
        }
        internal void clear() {
            Arrays.fill(status, 0, _size, (byte) 0);
            Arrays.fill(entries, 0, _size, null);
            Arrays.fill(deps, 0, _size, null);
            lowDep = -1;
            _size = 0;
        }
        internal int size() {
            return _size;
        }
        private void grow() {
            int newCapacity = (entries.Length << 1) + 1;
            byte[] newStatus = new byte[newCapacity];
            Object[] newEntries = new Object[newCapacity];
            HandleList[] newDeps = new HandleList[newCapacity];
            System.Array.Copy(status, 0, newStatus, 0, _size);
            System.Array.Copy(entries, 0, newEntries, 0, _size);
            System.Array.Copy(deps, 0, newDeps, 0, _size);
            status = newStatus;
            entries = newEntries;
            deps = newDeps;
        }
        private class HandleList {
            private int[] list = new int[4];
            private int _size = 0;
            public HandleList() {
            }
            public void add(int handle) {
                if (_size >= list.Length) {
                    int[] newList = new int[list.Length << 1];
                    System.Array.Copy(list, 0, newList, 0, list.Length);
                    list = newList;
                }
                list[_size++] = handle;
            }
            public int get(int index) {
                if (index >= _size) {
                    throw new Exception();
                }
                return list[index];
            }
            public int size() {
                return _size;
            }
        }
    }
    //private static Object cloneArray(Object array) {
    //    if (array is Object[]) {
    //        return ((Object[]) array).clone();
    //    } else if (array is boolean[]) {
    //        return ((boolean[]) array).clone();
    //    } else if (array is byte[]) {
    //        return ((byte[]) array).clone();
    //    } else if (array is char[]) {
    //        return ((char[]) array).clone();
    //    } else if (array is double[]) {
    //        return ((double[]) array).clone();
    //    } else if (array is float[]) {
    //        return ((float[]) array).clone();
    //    } else if (array is int[]) {
    //        return ((int[]) array).clone();
    //    } else if (array is long[]) {
    //        return ((long[]) array).clone();
    //    } else if (array is short[]) {
    //        return ((short[]) array).clone();
    //    } else {
    //        throw new AssertionError();
    //    }
    //}
}
}
