using System.Collections.Generic;

namespace BuildRadiator.Model {
  public class Message {
    public Message() {
    }

    public Message( string content, params string[] classes ) {
      Content = content;
      Classes = classes;
    }

    public string Content { get; set; }
    public ICollection<string> Classes { get; set; }
  }
}