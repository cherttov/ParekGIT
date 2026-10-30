export const branch = $state({
    name: "",
    revision: "",
    isDetached: false,
    hasUpstream: true,
    remoteBranch: "",
    commitsAhead: 0,
    commitsBehind: 0
});