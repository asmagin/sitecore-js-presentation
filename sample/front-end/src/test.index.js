var context = require.context('.', true, /spec\.(js|jsx)$/);
context.keys().forEach(context);
