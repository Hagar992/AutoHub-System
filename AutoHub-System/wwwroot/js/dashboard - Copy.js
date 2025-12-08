// Car Form JavaScript
document.addEventListener('DOMContentLoaded', function () {
    initializeCarForm();
});

function initializeCarForm() {
    const form = document.querySelector('.car-form');
    const mainImageInput = document.querySelector('input[asp-for="MainImage"]');
    const additionalImagesInput = document.querySelector('input[asp-for="Images"]');

    // Initialize file upload handlers
    initializeFileUpload(mainImageInput, additionalImagesInput);

    // Initialize form validation
    initializeFormValidation(form);

    // Initialize real-time validation
    initializeRealTimeValidation();

    console.log('Car form initialized successfully');
}

function initializeFileUpload(mainImageInput, additionalImagesInput) {
    // Main image upload handler
    if (mainImageInput) {
        const mainUploadArea = mainImageInput.nextElementSibling;

        mainImageInput.addEventListener('change', function (e) {
            handleMainImageUpload(this, mainUploadArea);
        });

        // Add drag and drop functionality
        addDragAndDrop(mainImageInput, mainUploadArea);
    }

    // Additional images upload handler
    if (additionalImagesInput) {
        const additionalUploadArea = additionalImagesInput.nextElementSibling;

        additionalImagesInput.addEventListener('change', function (e) {
            handleAdditionalImagesUpload(this, additionalUploadArea);
        });

        // Add drag and drop functionality
        addDragAndDrop(additionalImagesInput, additionalUploadArea);
    }
}

function handleMainImageUpload(input, uploadArea) {
    const file = input.files[0];
    if (!file) return;

    // Validate file
    if (!validateFile(file, 5)) { // 5MB limit
        input.value = '';
        return;
    }

    // Update upload area appearance
    updateUploadArea(uploadArea, file.name, 'success');

    // Create preview
    createImagePreview(file, uploadArea.parentElement, true);
}

function handleAdditionalImagesUpload(input, uploadArea) {
    const files = input.files;
    if (!files.length) return;

    // Validate files
    if (!validateMultipleFiles(files, 5, 10)) { // 5MB limit, max 10 files
        input.value = '';
        return;
    }

    // Update upload area appearance
    updateUploadArea(uploadArea, `${files.length} files selected`, 'success');

    // Create previews
    const container = uploadArea.parentElement;
    const existingPreviews = container.querySelector('.file-preview');
    if (existingPreviews) {
        existingPreviews.remove();
    }

    const previewContainer = document.createElement('div');
    previewContainer.className = 'file-preview';

    Array.from(files).forEach((file, index) => {
        createImagePreview(file, previewContainer, false, index);
    });

    container.appendChild(previewContainer);
}

function validateFile(file, maxSizeMB) {
    if (file.size > maxSizeMB * 1024 * 1024) {
        showNotification(`File "${file.name}" exceeds ${maxSizeMB}MB limit`, 'error');
        return false;
    }

    if (!file.type.startsWith('image/')) {
        showNotification(`File "${file.name}" is not an image`, 'error');
        return false;
    }

    return true;
}

function validateMultipleFiles(files, maxSizeMB, maxFiles) {
    if (files.length > maxFiles) {
        showNotification(`Maximum ${maxFiles} files allowed`, 'error');
        return false;
    }

    for (let file of files) {
        if (!validateFile(file, maxSizeMB)) {
            return false;
        }
    }

    return true;
}

function createImagePreview(file, container, isMain, index = 0) {
    const reader = new FileReader();

    reader.onload = function (e) {
        const previewItem = document.createElement('div');
        previewItem.className = 'preview-item';
        previewItem.dataset.index = index;

        const img = document.createElement('img');
        img.src = e.target.result;
        img.alt = file.name;

        const removeBtn = document.createElement('button');
        removeBtn.className = 'remove-btn';
        removeBtn.innerHTML = '×';
        removeBtn.title = 'Remove image';
        removeBtn.addEventListener('click', function () {
            removeImagePreview(previewItem, isMain);
        });

        previewItem.appendChild(img);
        previewItem.appendChild(removeBtn);
        container.appendChild(previewItem);
    };

    reader.readAsDataURL(file);
}

function removeImagePreview(previewItem, isMain) {
    previewItem.remove();

    if (isMain) {
        const mainInput = document.querySelector('input[asp-for="MainImage"]');
        mainInput.value = '';
        resetUploadArea(mainInput.nextElementSibling);
    } else {
        // For additional images, we need to update the file input
        updateFileInputAfterRemoval();
    }
}

function updateFileInputAfterRemoval() {
    // This would require a more complex implementation
    // to handle multiple file removal from input
    console.log('File removed - would update file input here');
}

function addDragAndDrop(input, uploadArea) {
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        uploadArea.addEventListener(eventName, preventDefaults, false);
    });

    function preventDefaults(e) {
        e.preventDefault();
        e.stopPropagation();
    }

    ['dragenter', 'dragover'].forEach(eventName => {
        uploadArea.addEventListener(eventName, highlight, false);
    });

    ['dragleave', 'drop'].forEach(eventName => {
        uploadArea.addEventListener(eventName, unhighlight, false);
    });

    function highlight() {
        uploadArea.style.borderColor = 'var(--primary)';
        uploadArea.style.backgroundColor = 'rgba(115, 128, 236, 0.1)';
    }

    function unhighlight() {
        uploadArea.style.borderColor = '';
        uploadArea.style.backgroundColor = '';
    }

    uploadArea.addEventListener('drop', handleDrop, false);

    function handleDrop(e) {
        const dt = e.dataTransfer;
        const files = dt.files;
        input.files = files;

        // Trigger change event
        const event = new Event('change', { bubbles: true });
        input.dispatchEvent(event);
    }
}

function updateUploadArea(uploadArea, text, status) {
    const icon = uploadArea.querySelector('span');
    const message = uploadArea.querySelector('p');
    const hint = uploadArea.querySelector('small');

    if (status === 'success') {
        icon.style.color = 'var(--success)';
        icon.textContent = 'check_circle';
    }

    message.textContent = text;
    hint.textContent = 'File ready for upload';
}

function resetUploadArea(uploadArea) {
    const icon = uploadArea.querySelector('span');
    const message = uploadArea.querySelector('p');
    const hint = uploadArea.querySelector('small');

    icon.style.color = '';
    icon.textContent = uploadArea.parentElement.classList.contains('main-image') ? 'image' : 'collections';
    message.textContent = uploadArea.parentElement.classList.contains('main-image') ?
        'Click to upload main image' : 'Click to upload multiple images';
    hint.textContent = uploadArea.parentElement.classList.contains('main-image') ?
        'Only one image' : 'Multiple images allowed';
}

function initializeFormValidation(form) {
    form.addEventListener('submit', function (e) {
        if (!validateForm(this)) {
            e.preventDefault();
            showNotification('Please fix the errors before submitting', 'error');
        } else {
            // Show loading state
            showLoadingState(true);
        }
    });
}

function initializeRealTimeValidation() {
    // Add real-time validation for required fields
    const requiredInputs = document.querySelectorAll('[required]');

    requiredInputs.forEach(input => {
        input.addEventListener('blur', function () {
            validateField(this);
        });

        input.addEventListener('input', function () {
            clearFieldError(this);
        });
    });
}

function validateField(field) {
    const value = field.value.trim();
    const isRequired = field.hasAttribute('required');

    if (isRequired && !value) {
        showFieldError(field, 'This field is required');
        return false;
    }

    // Add specific field validations
    switch (field.type) {
        case 'number':
            if (field.min && parseFloat(value) < parseFloat(field.min)) {
                showFieldError(field, `Value must be at least ${field.min}`);
                return false;
            }
            if (field.max && parseFloat(value) > parseFloat(field.max)) {
                showFieldError(field, `Value must be at most ${field.max}`);
                return false;
            }
            break;
        case 'file':
            // File validation handled separately
            break;
    }

    clearFieldError(field);
    return true;
}

function showFieldError(field, message) {
    clearFieldError(field);

    field.style.borderColor = 'var(--danger)';

    const errorElement = document.createElement('div');
    errorElement.className = 'validation-message';
    errorElement.textContent = message;

    field.parentNode.appendChild(errorElement);
}

function clearFieldError(field) {
    field.style.borderColor = '';

    const existingError = field.parentNode.querySelector('.validation-message');
    if (existingError) {
        existingError.remove();
    }
}

function validateForm(form) {
    let isValid = true;
    const requiredFields = form.querySelectorAll('[required]');

    requiredFields.forEach(field => {
        if (!validateField(field)) {
            isValid = false;
        }
    });

    // Additional form-level validation
    const mainImageInput = form.querySelector('input[asp-for="MainImage"]');
    if (mainImageInput && !mainImageInput.files.length) {
        showFieldError(mainImageInput, 'Main image is required');
        isValid = false;
    }

    return isValid;
}

function showLoadingState(show) {
    const submitBtn = document.querySelector('.btn-primary');

    if (show) {
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<div class="loading-spinner"></div> Adding Car...';
        submitBtn.classList.add('loading');
    } else {
        submitBtn.disabled = false;
        submitBtn.innerHTML = '<span class="material-symbols-sharp">add_circle</span> Add Car';
        submitBtn.classList.remove('loading');
    }
}

function showNotification(message, type = 'info') {
    // Remove existing notifications
    const existingNotifications = document.querySelectorAll('.custom-notification');
    existingNotifications.forEach(notification => notification.remove());

    const notification = document.createElement('div');
    notification.className = `custom-notification ${type}`;
    notification.innerHTML = `
        <span class="material-symbols-sharp">${getNotificationIcon(type)}</span>
        <p>${message}</p>
    `;

    // Add styles for notification
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: var(--white);
        padding: 1rem 1.5rem;
        border-radius: var(--border-radius-1);
        box-shadow: var(--box-shadow);
        display: flex;
        align-items: center;
        gap: 0.5rem;
        z-index: 1000;
        border-left: 4px solid ${getNotificationColor(type)};
        animation: slideIn 0.3s ease-out;
    `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease-in';
        setTimeout(() => notification.remove(), 300);
    }, 5000);
}

function getNotificationIcon(type) {
    const icons = {
        'success': 'check_circle',
        'error': 'error',
        'warning': 'warning',
        'info': 'info'
    };
    return icons[type] || 'info';
}

function getNotificationColor(type) {
    const colors = {
        'success': 'var(--success)',
        'error': 'var(--danger)',
        'warning': 'var(--warning)',
        'info': 'var(--primary)'
    };
    return colors[type] || 'var(--primary)';
}

// Add CSS animations for notifications
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    
    @keyframes slideOut {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(style);

// Export functions for global access
window.CarForm = {
    initializeCarForm,
    validateForm,
    showNotification
};