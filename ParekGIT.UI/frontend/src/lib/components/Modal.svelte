<script>
    let {
        open = false,
        title = "", // Modal header title
        confirmText = "Confirm", // Text for confirm button (null/empty to hide)
        cancelText = "Cancel", // Text for cancel btn (null/empty to hide)
        confirmDisabled = false, // Disable confirm btn
        closeOnBackdrop = true, // Close when backdrop clicked
        closeOnEscape = true, // Close when ESC pressed
        bodyStyle = "", // Inline styles for modal-body (e.g. padding 0 for lists)
        onconfirm, // Confirm callback
        oncancel, // Cancel callback
        onclose, // Close callback
        children, // Modal body content
        footer // Custom footer
    } = $props();

    function close() {
        oncancel?.();
        onclose?.();
    }

    function handleBackdropClick(event) { 
        if (closeOnBackdrop && event.target === event.currentTarget) {
            close();
        }
    }

    function handleKeydown(event) {
        if (open && closeOnEscape && event.key === "Escape") {
            event.stopPropagation();
            close();
        }
    }
</script>

<svelte:window onkeydown={open ? handleKeydown : undefined} />
<!-- svelte-ignore a11y_click_events_have_key_events -->
<!-- svelte-ignore a11y_no_static_element_interactions -->
<div class="modal-backdrop" class:show={open} onclick={handleBackdropClick}>
    <div class="modal-content" onclick={(e) => e.stopPropagation()}>
        <div class="modal-header">
            <h3>{title}</h3>
            <button class="close-modal-icon" onclick={close}>&times;</button>
        </div>

        <div class="modal-body" style={bodyStyle}>
            {@render children?.()}
        </div>

        <div class="modal-footer">
            {#if footer}
                {@render footer()}
            {:else}
                {#if confirmText}
                    <button class="confirm-modal-btn" disabled={confirmDisabled} onclick={onconfirm}>{confirmText}</button>
                {/if}
                {#if cancelText}
                    <button class="cancel-modal-btn" onclick={close}>{cancelText}</button>
                {/if}
            {/if}
        </div>
    </div>
</div>