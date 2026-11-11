export const history = $state({
    commits: [],
    selectedHash: "",
    fetching: false,
    reachedEnd: false,
    skip: 0,
    take: 50,

    setCommits(list) {
        this.commits = list ?? [];
        this.skip = 0;
        this.fetching = false;
        this.reachedEnd = this.commits.length < this.take;
    },
    appendCommits(list) {
        const next = list ?? [];
        this.commits = [...this.commits, ...next];
        this.fetching = false;
        if (next.length < this.take) this.reachedEnd = true;
    },
    beginNextPage() {
        if (this.fetching || this.reachedEnd) return null;
        this.fetching = true;
        this.skip += this.take;
        return this.skip;
    },
    select(hash) { this.selectedHash = hash; },
    reset() { 
        this.commits = [];
        this.selectedHash = "";
        this.fetching = false;
        this.skip = 0;
    }
});