const binaryExts = [
	".exe", ".dll", ".bin", ".obj",
	".pdb", ".png", ".jpg", ".jpeg",
	".gif", ".ico", ".zip", ".tar",
	".gz", ".7z", ".pdf",
];

export function isBinary(filePath) {
    const lower = filePath.toLowerCase();
    return binaryExts.some(ext => lower.endsWith(ext));
}

export function parseDiff(diffText) {
    if (!diffText || diffText.trim() === "") return null;
    if (diffText.includes("Binary files ")) return "binary";
    
    const lines = diffText.split("\n");
    const result = [];
    let hitFirstChunk = !diffText.startsWith("diff --git");

    for (const line of lines) {
        if (!hitFirstChunk) {
            if (line.startsWith("@@")) hitFirstChunk = true;
            else continue;
        }
        if (line.startsWith("\\")) continue;

        if (line.startsWith("+")) {
            result.push({ type: "add", text: "+ " + line.substring(1) });
        } else if (line.startsWith("-")) {
            result.push({ type: "remove", text: "- " + line.substring(1) });
        } else if (line.startsWith("@@")) {
            result.push({ type: "chunk", text: line.substring(1) });
        } else {
            result.push({ type: "normal", text: "  " + line.substring(1) });
        }
    }

    if (result.length === 0) return "empty";
    return result;
}