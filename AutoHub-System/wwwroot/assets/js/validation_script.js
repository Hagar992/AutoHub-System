document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('payForm');
    const fields = ['name', 'email', 'phone', 'phone2', 'ssn'];

    // قواعد التحقق (مستمدة من PaymentViewModel)
    const validationRules = {
        name: {
            required: true,
            minLength: 3,
            maxLength: 100,
            requiredMessage: "Name is required",
            minLengthMessage: "Name can not be less 3 characters",
            maxLengthMessage: "Name can not exceed 100 characters"
        },
        email: {
            required: true,
            regex: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, // تبسيط لـ EmailAddressAttribute
            requiredMessage: "Email is required",
            regexMessage: "Invalid email format should be like yourName12@gmail.com"
        },
        phone: {
            required: true,
            regex: /^01[0125][0-9]{8}$/, // Egyptian phone number regex
            requiredMessage: "Phone number is required",
            regexMessage: "Invalid Egyptian phone number"
        },
        phone2: {
            required: false, // اختياري
            regex: /^01[0125][0-9]{8}$/, // Egyptian phone number regex
            regexMessage: "Invalid Egyptian phone number"
        },
        ssn: {
            required: true,
            regex: /^\d{14}$/, // 14 digits National ID regex
            requiredMessage: "National ID is required",
            regexMessage: "National ID must be exactly 14 digits"
        }
    };

    /**
     * تعرض أو تخفي رسالة الخطأ لحقل معين.
     * @param {string} fieldId - معرف الحقل (مثل 'name').
     * @param {string} message - رسالة الخطأ المراد عرضها.
     */
    function displayError(fieldId, message) {
        const errorElement = document.getElementById(`${fieldId}-error`);
        const inputElement = document.getElementById(fieldId);

        if (errorElement) {
            errorElement.textContent = message;
            if (message) {
                // إضافة فئة لتنسيق الحقل عند وجود خطأ
                inputElement.classList.add('input-error');
            } else {
                inputElement.classList.remove('input-error');
            }
        }
    }

    /**
     * تتحقق من صحة حقل إدخال واحد.
     * @param {HTMLElement} input - عنصر الإدخال (input element).
     * @returns {boolean} - true إذا كان الحقل صحيحًا، false إذا كان خاطئًا.
     */
    function validateField(input) {
        const fieldId = input.id;
        const value = input.value.trim();
        const rules = validationRules[fieldId];
        let errorMessage = '';

        // 1. Required check
        if (rules.required && value === '') {
            errorMessage = rules.requiredMessage;
        }

        // 2. MinLength check (فقط لو الحقل مش فارغ)
        else if (rules.minLength && value.length > 0 && value.length < rules.minLength) {
            errorMessage = rules.minLengthMessage;
        }

        // 3. MaxLength check
        else if (rules.maxLength && value.length > rules.maxLength) {
            errorMessage = rules.maxLengthMessage;
        }

        // 4. Regex check (لو موجود ومش فارغ)
        else if (rules.regex && value !== '' && !rules.regex.test(value)) {
            errorMessage = rules.regexMessage;
        }

        displayError(fieldId, errorMessage);
        return errorMessage === '';
    }
    // تعيين مستمعي الأحداث لحدث 'blur' لكل حقل
    fields.forEach(fieldId => {
        const input = document.getElementById(fieldId);
        if (input) {
            // التحقق عند الخروج من الحقل (on blur)
            input.addEventListener('blur', () => {
                validateField(input);
            });

            // إزالة رسالة الخطأ عند بدء الكتابة مرة أخرى (اختياري لتحسين تجربة المستخدم)
            input.addEventListener('input', () => {
                // إزالة رسالة الخطأ فقط إذا كانت موجودة
                if (document.getElementById(`${fieldId}-error`).textContent) {
                    displayError(fieldId, '');
                }
            });
        }
    });

    // التحقق من صحة جميع الحقول عند محاولة إرسال النموذج
    form.addEventListener('submit', (event) => {
        let isFormValid = true;
        fields.forEach(fieldId => {
            const input = document.getElementById(fieldId);
            // نستخدم && هنا لضمان أن isFormValid تصبح false إذا فشل أي حقل
            if (input) {
                isFormValid = validateField(input) && isFormValid;
            }
        });

        if (!isFormValid) {
            event.preventDefault(); 
            document.getElementById('message').textContent = 'Please correct the errors in the form.';
        } else {
            document.getElementById('message').textContent = '';
        }
    });
});
