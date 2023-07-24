using System;
using Bev.IO.RemoteInterface;
using NationalInstruments.Visa;

namespace Bev.IO.Gpib.NiVisa
{
    public class NiVisa : IRemoteInterface
    {

        private GpibSession session;
        private int initAddress;

        public NiVisa(int address)
        {
            CheckIfAddressInRange(address);
            initAddress = address;
            ResourceManager resourceManager = new ResourceManager();
            session = (GpibSession)resourceManager.Open($"GPIB0::{address}::INSTR");
            session.TerminationCharacterEnabled = true;
        }

        public string Enter(int address)
        {
            CheckIfAddressConsistent(address);
            return session.RawIO.ReadString();
        }

        public void Local(int address)
        {
            CheckIfAddressConsistent(address);
            Nop();
        }

        public void Output(int address, string command)
        {
            CheckIfAddressConsistent(address);
            session.RawIO.Write(command);
        }

        public void Remote(int address)
        {
            CheckIfAddressConsistent(address);
            Nop();
        }

        public void Trigger(int address)
        {
            CheckIfAddressConsistent(address);
            session.AssertTrigger();
        }

        private void CheckIfAddressInRange(int address)
        {
            if (address < 0) throw new ArgumentOutOfRangeException("IEEE 488 address cannot be negative");
            if (address > 31) throw new ArgumentOutOfRangeException("IEEE 488 address cannot be larger than 31");
        }

        private void CheckIfAddressConsistent(int address)
        {
            if (address != initAddress) throw new ArgumentException("Address different from initialisation");
        }

        private void Nop()
        { }
    }
}
