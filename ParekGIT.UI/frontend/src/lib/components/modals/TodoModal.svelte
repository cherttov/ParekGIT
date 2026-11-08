<script>
    import { untrack } from "svelte";
    import Modal from "../Modal.svelte";
    import CustomScrollbar from "../CustomScrollbar.svelte";
    import { todos } from "../../stores/todos.svelte";
    import { saveTodos } from "../../ipc/commands";

    let { open = false, data = {}, onclose } = $props();
    let task = $state("");
    let draft = $state([]);
    let listEl = $state("");
    let inputEl;

    const canAdd = $derived(task.trim() !== "");

    $effect(() => {
        if (!open) return;
        task = "";
        draft = untrack(() => $state.snapshot(todos.list));
        const t = setTimeout(() => inputEl.focus(), 100);
        return () => clearTimeout(t);
    });

    function addTask() {
        if (!canAdd) return;
        draft.push({ taskLabel: task.trim(), isCompleted: false });
        task = "";
        inputEl.focus();
    }

    function removeTask(index) {
        draft.splice(index, 1);
    }

    function confirm() {
        const list = $state.snapshot(draft);
        todos.list = list;
        saveTodos(list);
        onclose?.();
    }

    function handleKeydown(e) {
        if (e.key === "Enter" && !e.isComposing) addTask();
    }
</script>

<Modal
    {open}
    title="To Do List"
    confirmText="Save"
    bodyStyle="padding: 0; gap: 0"
    onconfirm={confirm}
    {onclose}
>
    <div class="modal-list-wrapper">
        <div class="stash-new-row">
            <input type="text" class="modal-input" id="todo-modal-input-task" placeholder="Add new task (press enter or +)"
                bind:value={task} bind:this={inputEl} onkeydown={handleKeydown} />
            <button class="stash-action-btn stash-save-btn" id="todo-modal-add-btn" disabled={!canAdd} onclick={addTask}>+</button>
        </div>

        <div class="modal-list-rows" id="todo-rows-container" bind:this={listEl}>
            {#if draft.length === 0}
                <div class="stash-empty">No todos in this repository</div>
            {:else}
                {#each draft as item, i (item)}
                    <div class="todo-modal-row">
                        <input type="checkbox" class="ui-checkbox todo-checkbox" bind:checked={item.isCompleted} />
                        <span class="todo-modal-label" class:done={item.isCompleted}>{item.taskLabel}</span>
                        <button class="todo-modal-remove-btn" onclick={() => removeTask(i)}>&times;</button>
                    </div>
                {/each}
            {/if}
        </div>
        
        <CustomScrollbar container={listEl}></CustomScrollbar>
    </div>
</Modal>