using codecrafters_git.Enums;

namespace codecrafters_git.Models;

public class TreeObjectEntry {
    public TreeObjectMode Mode { get; set; }
    public string Name { get; set; }
    public ShaOne Sha1 { get; set; }
}