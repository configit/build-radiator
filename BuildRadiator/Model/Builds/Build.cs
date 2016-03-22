using System;
using System.Collections.Generic;

namespace BuildRadiator.Model.Builds {
  public class Build {
    public string Name { get; set; }
    public string BranchName { get; set; }
    public BuildStatus Status { get; set; }
    public string StatusText { get; set; }
    public string StatusSubText { get; set; }
    public bool PreviouslyFailing { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public int PercentComplete { get; set; }
    public string Investigator { get; set; }
    public ICollection<string> Committers { get; set; }

    public Build() {
      Committers = new List<string>();
    }
  }
}