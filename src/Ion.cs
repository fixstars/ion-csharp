using System;
using System.Runtime.InteropServices;

public class Ion
{
    public enum TypeCode
    {
        Int = 0,   //!< signed integers
        Uint = 1,  //!< unsigned integers
        Float = 2, //!< floating point numbers
        Handle = 3 //!< opaque pointer type (void *)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Type
    {
        TypeCode code;
        Byte bits;
        Byte lanes;

        public Type(TypeCode code_, Byte bits_, Byte lanes_)
        {
            code = code_;
            bits = bits_;
            lanes = lanes_;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BuilderCompileOPtion
    {
        string output_directory;
    }


    private static class NativeMethods
    {
        [DllImport("ion-core.dll")]
        internal static extern Int32 ion_port_create(IntPtr ptr, String key, Type type);

        [DllImport("ion-core.dll")]
        internal static extern Int32 ion_port_destroy(IntPtr ptr);
    }

    public class Port
    {
        private IntPtr handle;

        public Port(String key, Type type)
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_port_create(ptr, key, type);
            if (ret != 0) {
                throw new InvalidOperationException();
            }
            handle = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);
        }

        ~Port()
        {
            if (handle !=IntPtr.Zero)
            {
                NativeMethods.ion_port_destroy(handle);
    }
        }
    }

}