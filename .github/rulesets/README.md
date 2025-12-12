# GitHub Rulesets

This directory contains ruleset configurations for branch protection.

## Applying the Ruleset

### Option 1: Import via GitHub UI

1. Go to **Repository Settings** → **Rules** → **Rulesets**
2. Click **New ruleset** → **Import a ruleset**
3. Upload `branch-protection.json`
4. Review and click **Create**

### Option 2: Create Manually via GitHub UI

1. Go to **Repository Settings** → **Rules** → **Rulesets**
2. Click **New ruleset** → **New branch ruleset**
3. Configure as follows:

#### General Settings
- **Ruleset name**: `Protected Branches`
- **Enforcement status**: `Active`

#### Target Branches
Add these branch patterns:
- `main`
- `release/net8`
- `release/net9`

#### Branch Rules
Enable the following rules:

| Rule | Setting |
|------|---------|
| **Restrict deletions** | ✅ Enabled |
| **Block force pushes** | ✅ Enabled |
| **Require a pull request before merging** | ✅ Enabled |
| ↳ Required approvals | `1` |
| ↳ Dismiss stale reviews | ✅ Enabled |
| ↳ Require review from code owners | ❌ Disabled |
| ↳ Require conversation resolution | ✅ Enabled |
| **Require status checks to pass** | ✅ Enabled |
| ↳ Require branches to be up to date | ✅ Enabled |
| ↳ Status checks: | `build` |

#### Bypass List (Optional)
- Repository admins can bypass (for emergency fixes)

### Option 3: GitHub CLI

```bash
gh api repos/{owner}/{repo}/rulesets \
  --method POST \
  --input .github/rulesets/branch-protection.json
```

## Ruleset Summary

This ruleset protects the following branches:
- `main` (.NET 10 LTS)
- `release/net8` (.NET 8 LTS)
- `release/net9` (.NET 9 STS)

**Protection includes:**
- No direct pushes (PRs required)
- At least 1 approval required
- CI must pass before merging
- No force pushes
- No branch deletion
- Stale reviews dismissed on new commits
