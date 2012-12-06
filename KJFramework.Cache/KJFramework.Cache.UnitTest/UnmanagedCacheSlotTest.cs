using System;
using System.Text;
using KJFramework.Cache.Cores;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Cache.UnitTest
{
    [TestClass]
    public class UnmanagedCacheSlotTest
    {
        [TestMethod]
        public void NewTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024);
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
        }

        [TestMethod]
        public void NewWithTimeoutTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024, DateTime.Now.AddMinutes(5));
            Assert.IsNotNull(slot);
            Assert.IsTrue(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
        }

        [TestMethod]
        public void GetValueWithInitializeDataTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024);
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
            byte[] data = slot.GetValue();
            Assert.IsTrue(data.Length == 1024);
        }

        [TestMethod]
        public void GetValueTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024);
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
            slot.SetValue(Encoding.Default.GetBytes("1234567890"));
            byte[] data = slot.GetValue();
            Assert.IsTrue(data.Length == 10);
            Assert.IsTrue(Encoding.Default.GetString(data) == "1234567890");
        }


        [TestMethod]
        public void SetValueTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024);
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
            slot.SetValue(Encoding.Default.GetBytes("i love you forever."));
        }

        [TestMethod]
        public void DiscardTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024);
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
            slot.Discard();
            Assert.IsTrue(slot.GetLease().IsDead);
        }

        [TestMethod]
        public void GetHandleTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024);
            Assert.IsNotNull(slot);
            Assert.IsFalse(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
            Assert.IsTrue(slot.Handle != IntPtr.Zero);
            slot.Discard();
            Assert.IsTrue(slot.Handle == IntPtr.Zero);
        }

        [TestMethod]
        public void CreateSlotByHandleTest()
        {
            IUnmanagedCacheSlot slot = UnmanagedCacheSlot.New(1024);
            Assert.IsFalse(slot.GetLease().CanTimeout);
            Assert.IsFalse(slot.GetLease().IsDead);
            Assert.IsTrue(slot.Handle != IntPtr.Zero);
            slot.SetValue(Encoding.Default.GetBytes("0123456789"));
            IUnmanagedCacheSlot newSlot = UnmanagedCacheSlot.New(slot.Handle, 1024, 10);
            Assert.IsFalse(newSlot.GetLease().CanTimeout);
            Assert.IsFalse(newSlot.GetLease().IsDead);
            Assert.IsTrue(newSlot.Handle != IntPtr.Zero);
            Assert.IsTrue(Encoding.Default.GetString(newSlot.GetValue()) == "0123456789");
        }
    }
}