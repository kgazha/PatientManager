using System;
using System.Collections.Generic;
using System.Linq;

namespace PatientManager
{
    internal class PatientInfo
    {
        public IEnumerable<dynamic> Receptions { get; set; }
        public IEnumerable<dynamic> Patients { get; set; }

        public PatientInfo()
        {
            var rand = new Random();

            Receptions = Enumerable.Range(1, 500000).SelectMany(pid =>
                Enumerable.Range(1, rand.Next(0, 100)).Select(rid => new
                {
                    PatientId = pid,
                    ReceptionStart = new DateTime(2017, 06, 30).AddDays(-rand.Next(1, 500))
                })).ToList();

            Patients = Enumerable.Range(1, 500000)
                .Select(pid => new
                {
                    Id = pid,
                    Surname = string.Format("Иванов{0}", pid)
                }).ToList();
        }

        public IEnumerable<dynamic> GetPatientsFirstCase(DateTime thresholdDateTime)
            => Receptions.Join(Patients, r => r.PatientId, p => p.Id,
                (r, p) => new { Surname = p.Surname, ReceptionStart = r.ReceptionStart })
                .Where(i => i.ReceptionStart < thresholdDateTime).ToList();

        public IEnumerable<dynamic> GetPatientsSecondCase(DateTime thresholdDateTime)
        {
            var filteredPatientIds = Receptions.Where(i => i.ReceptionStart < thresholdDateTime)
                .Select(i => i.PatientId).ToHashSet();

            return Patients.Where(p => filteredPatientIds.Contains(p.Id)).ToList();
        }

        public IEnumerable<dynamic> GetPatientsThirdCase(DateTime thresholdDateTime)
        {
            var filteredPatientIds = Receptions.Where(i => i.ReceptionStart < thresholdDateTime)
                .Select(i => i.PatientId).ToList();

            return Patients.Where(p => filteredPatientIds.Contains(p.Id)).ToList();
        }
    }
}
