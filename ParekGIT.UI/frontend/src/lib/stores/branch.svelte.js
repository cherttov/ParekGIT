export const branch = $state({
    name: "",
    revision: "",
    isDetached: false,
    hasUpstream: false,
    remoteBranch: "",
    commitsAhead: 0,
    commitsBehind: 0
});