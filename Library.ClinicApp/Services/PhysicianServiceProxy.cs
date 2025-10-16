using System;
using Library.ClinicApp.Models;

namespace Library.ClinicApp.Services;

public class PhysicianServiceProxy
{
    private List<Physician?> physicians;
    private PhysicianServiceProxy()
    {
        physicians = new List<Physician?>();
    }
    private static PhysicianServiceProxy? instance;
    private static object instanceLock = new object();
    public static PhysicianServiceProxy Current
    {
        get
        {
            lock(instanceLock)
            { 
                if (instance == null)
                {
                    instance = new PhysicianServiceProxy();
                }
            }

            return instance;
        }
    }

    public List<Physician?> Physicians
    {
        get
        {
            return physicians;
        }
    }

    public Physician? AddOrUpdate(Physician? physician)
    {
        if (physician == null)
        {
            return null;
        }
        var existingPhysician = Physicians.FirstOrDefault(p => p?.Id == physician.Id);
        if (existingPhysician != null) // existing physician found, so edit instead of add
        {
            var index = Physicians.IndexOf(existingPhysician);
            Physicians.RemoveAt(index);
            Physicians.Insert(index, physician);
        }
        else
        {
            physicians.Add(physician); // new physician
        }
        return physician;
    }

    public Physician? Delete(string id)
    {
        var physicianToDelete = physicians.Where(b => b != null).FirstOrDefault(b => (b?.Id ?? "") == id);
        physicians.Remove(physicianToDelete);
        return physicianToDelete;
    }
}
