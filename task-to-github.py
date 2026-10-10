import subprocess
import sys

# ==========================================
# 🛠️ ASETUKSET: MUUTA TÄMÄ TIIMIN REPOON!
# Formaatti: "käyttäjänimi/repon-nimi"
#labelit = bug documentation enhancement new feature security testing
# ==========================================
REPO = "btekki/LifeManagementApp"
PROJECT = "CSP-LifeManagementApp"

tickets = [
    {
        "title": "task-name-here",
        "body": (
            "### Task\n"
            "header\n\n"
            "### Requirements:\n"
            "- More details here.\n"
            "- Remember new line.\n"
           
        ),
        "labels": []
    },
   
]

def create_issues():
    print(f"Creating tasks for: {REPO}...")
    for t in tickets:
        cmd = ["gh", "issue", "create", "--repo", REPO, "--title", t["title"], "--body", t["body"], "--project", PROJECT,]

        for label in t.get("labels", []):
            cmd.extend(["--label", label])
        try:
            subprocess.run(cmd, check=True, capture_output=True, text=True)
            print(f" Created: {t['title']}")
        except subprocess.CalledProcessError as e:
            print(f" Error: {e.stderr}")
            sys.exit(1)
    print("\nTask setup complete!")

if __name__ == "__main__":
    if "your-username" in REPO:
        print("⚠️ MUISTA PÄIVITTÄÄ 'REPO' MUUTTUJA SKRIPTISSÄ!")
        sys.exit(1)
    create_issues()